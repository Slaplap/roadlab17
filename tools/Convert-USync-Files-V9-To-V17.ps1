<#
.SYNOPSIS
    Pre-processes uSync v17 Content config files to convert legacy
    property values (Nested Content, MediaPicker v1) to v17 format
    (Block List, MediaPicker3) so uSync's import can write them without
    rejection.

.DESCRIPTION
    The v17 Content/*.config files still hold v9-era JSON shapes inside
    each property's <Value> tag, e.g. Nested Content arrays where the
    editor is now Block List. uSync v17 validates against the editor's
    expected format and silently drops the property. Running this script
    transforms each affected <Value> in-place so a subsequent uSync
    force-import populates the DB cleanly.

    Conversions:
        Umbraco.NestedContent  -> Umbraco.BlockList     (array  -> object)
        Umbraco.MediaPicker    -> Umbraco.MediaPicker3  (UDI    -> JSON)
    Grid -> BlockGrid is not handled; those values are nulled instead
    (consistent with the in-DB cleanup already done).

    Reads the property alias -> editor mapping from the local DB so we
    know which property aliases need converting.

.PARAMETER ContentFolder
    Directory containing the uSync content config files. Defaults to
    Interon.Roadlab.Web.v17\uSync\v17\Content relative to repo root.

.PARAMETER ConnectionString
    SQL Server connection. Defaults to local LocalDB Roadlab_Local.

.PARAMETER DryRun
    Show what would change without writing back.

.EXAMPLE
    .\Convert-USync-Files-V9-To-V17.ps1 -DryRun
#>
[CmdletBinding()]
param(
    [string]$ContentFolder = "C:\dev\Roadlab\Interon.Roadlab.Web.v17\uSync\v17\Content",
    [string]$ConnectionString = "Server=(localdb)\MSSQLLocalDB;Database=Roadlab_Local;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=True;",
    [switch]$DryRun
)

Add-Type -AssemblyName System.Data
$ErrorActionPreference = 'Stop'

# Helpers
function Get-UdiFromKey([string]$key) { "umb://element/" + ($key -replace '-','') }

function ConvertTo-Mp3Item([string]$udi) {
    if ([string]::IsNullOrWhiteSpace($udi)) { return $null }
    if ($udi -notmatch 'umb://media/([0-9a-fA-F]{32}|[0-9a-fA-F\-]{36})') { return $null }
    $raw = $matches[1] -replace '-',''
    $mediaKey = "$($raw.Substring(0,8))-$($raw.Substring(8,4))-$($raw.Substring(12,4))-$($raw.Substring(16,4))-$($raw.Substring(20,12))"
    return [pscustomobject]@{ key = [guid]::NewGuid().ToString(); mediaKey = $mediaKey }
}

# 1) Build maps from DB
$conn = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)
$conn.Open()
$cmd = $conn.CreateCommand()

# alias -> editor (for the FIRST occurrence; collisions across content types are an accepted edge case)
$aliasToEditor = @{}
$cmd.CommandText = "SELECT pt.Alias, dt.propertyEditorAlias FROM cmsPropertyType pt INNER JOIN umbracoDataType dt ON pt.dataTypeId = dt.nodeId"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    $a = $reader.GetString(0); $e = $reader.GetString(1)
    if (-not $aliasToEditor.ContainsKey($a)) { $aliasToEditor[$a] = $e }
}
$reader.Close()

# content type alias -> key (for element types used inside NC)
$ctAliasToKey = @{}
$cmd.CommandText = "SELECT n.uniqueId, ct.alias FROM umbracoNode n INNER JOIN cmsContentType ct ON n.id = ct.nodeId WHERE n.nodeObjectType = 'A2CB7800-F571-4787-9638-BC48539A0EFB'"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) { $ctAliasToKey[$reader.GetString(1)] = $reader.GetGuid(0).ToString() }
$reader.Close()
$conn.Close()

Write-Host "Loaded $($aliasToEditor.Count) property-alias mappings and $($ctAliasToKey.Count) content-type-alias mappings" -ForegroundColor Green

# 2) Process files
$files = Get-ChildItem -Path $ContentFolder -Filter '*.config' -File
$filesChanged = 0
$valuesChanged = 0
$skipped = 0

foreach ($file in $files) {
    [xml]$xml = Get-Content -LiteralPath $file.FullName -Raw -Encoding UTF8
    if ($xml.DocumentElement.LocalName -eq 'Empty') { continue }
    $props = $xml.SelectSingleNode('//Content/Properties')
    if ($null -eq $props) { continue }

    $changed = $false
    foreach ($propNode in $props.ChildNodes) {
        if ($propNode.NodeType -ne [System.Xml.XmlNodeType]::Element) { continue }
        $alias = $propNode.LocalName
        if (-not $aliasToEditor.ContainsKey($alias)) { continue }
        $editor = $aliasToEditor[$alias]
        $valueNode = $propNode.SelectSingleNode('Value')
        if ($null -eq $valueNode) { continue }
        $raw = $valueNode.InnerText
        if ([string]::IsNullOrWhiteSpace($raw)) { continue }
        $trim = $raw.TrimStart()

        try {
            if ($editor -eq 'Umbraco.BlockList') {
                if ($trim.StartsWith('{')) { continue }  # already v17
                if (-not $trim.StartsWith('[')) { continue }  # not NC either
                $items = $raw | ConvertFrom-Json
                if ($null -eq $items) { continue }
                if (-not ($items -is [System.Array])) { $items = @($items) }
                $layout = @(); $contentData = @()
                foreach ($it in $items) {
                    $ncAlias = $it.ncContentTypeAlias
                    if (-not $ctAliasToKey.ContainsKey($ncAlias)) {
                        Write-Warning "[$($file.Name) / $alias] unknown element type '$ncAlias' - dropping item"; continue
                    }
                    $udi = Get-UdiFromKey $it.key
                    $layout += [pscustomobject]@{ contentUdi = $udi }
                    $data = [ordered]@{ contentTypeKey = $ctAliasToKey[$ncAlias]; udi = $udi }
                    foreach ($p in $it.PSObject.Properties) {
                        if ($p.Name -in @('key','name','ncContentTypeAlias')) { continue }
                        $v = $p.Value
                        # If this nested value looks like a v1 mediapicker UDI string, transform it
                        if ($v -is [string] -and $v -match '^umb://media/') {
                            $nested = @()
                            foreach ($u in ($v -split ',')) {
                                $mp = ConvertTo-Mp3Item $u.Trim()
                                if ($mp) { $nested += $mp }
                            }
                            if ($nested.Count -gt 0) { $v = ($nested | ConvertTo-Json -Depth 5 -Compress); if ($v -notmatch '^\[') { $v = "[$v]" } }
                        }
                        $data[$p.Name] = $v
                    }
                    $contentData += [pscustomobject]$data
                }
                $bl = [ordered]@{ layout = [ordered]@{ 'Umbraco.BlockList' = $layout }; contentData = $contentData; settingsData = @() }
                $newJson = $bl | ConvertTo-Json -Depth 25 -Compress
                $valueNode.InnerText = "`r`n" + $newJson + "`r`n"
                $changed = $true; $valuesChanged++
            }
            elseif ($editor -eq 'Umbraco.MediaPicker3') {
                if ($trim.StartsWith('[')) { continue }  # already v17
                $items = @()
                foreach ($u in ($raw -split ',')) {
                    $mp = ConvertTo-Mp3Item $u.Trim(); if ($mp) { $items += $mp }
                }
                if ($items.Count -eq 0) { continue }
                $newJson = $items | ConvertTo-Json -Depth 5 -Compress
                if ($newJson -notmatch '^\[') { $newJson = "[$newJson]" }
                $valueNode.InnerText = $newJson
                $changed = $true; $valuesChanged++
            }
            elseif ($editor -eq 'Umbraco.BlockGrid') {
                if ($trim.StartsWith('{') -and $trim -notmatch '"sections"') { continue }  # already BG
                if ($trim -match '"sections"') {
                    # Grid Layout -> Block Grid. We only have one block type (blogContentBlock,
                    # key 9a4f7c2e-1b3d-4e8a-bf01-7d62a8f5c930) with a single RTE field `content`.
                    # Flatten the grid's controls into one HTML string and put it into one block.
                    try {
                        $grid = $raw | ConvertFrom-Json
                        $sb = New-Object System.Text.StringBuilder
                        foreach ($section in @($grid.sections)) {
                            foreach ($row in @($section.rows)) {
                                foreach ($area in @($row.areas)) {
                                    foreach ($ctrl in @($area.controls)) {
                                        $editAlias = if ($ctrl.editor) { $ctrl.editor.alias } else { '' }
                                        switch ($editAlias) {
                                            'rte' {
                                                [void]$sb.AppendLine([string]$ctrl.value)
                                            }
                                            'textstring' {
                                                [void]$sb.AppendLine('<p>' + [string]$ctrl.value + '</p>')
                                            }
                                            'media' {
                                                $src = if ($ctrl.value.image) { [string]$ctrl.value.image } else { '' }
                                                if ($src) { [void]$sb.AppendLine('<p><img src="' + $src + '" /></p>') }
                                            }
                                            'embed' {
                                                if ($ctrl.value.preview) { [void]$sb.AppendLine([string]$ctrl.value.preview) }
                                                elseif ($ctrl.value) { [void]$sb.AppendLine('<!-- embed: ' + [string]$ctrl.value + ' -->') }
                                            }
                                            default {
                                                # Unknown control type - skip with comment
                                                if ($editAlias) { [void]$sb.AppendLine('<!-- ' + $editAlias + ' control dropped -->') }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        $blockUdi = "umb://element/" + ([guid]::NewGuid().ToString() -replace '-','')
                        $bg = [ordered]@{
                            layout = [ordered]@{
                                'Umbraco.BlockGrid' = @([ordered]@{ contentUdi = $blockUdi; areas = @(); columnSpan = 12; rowSpan = 1 })
                            }
                            contentData = @([ordered]@{
                                contentTypeKey = '9a4f7c2e-1b3d-4e8a-bf01-7d62a8f5c930'
                                udi = $blockUdi
                                content = $sb.ToString()
                            })
                            settingsData = @()
                        }
                        $newJson = $bg | ConvertTo-Json -Depth 25 -Compress
                        $valueNode.InnerText = "`r`n" + $newJson + "`r`n"
                        $changed = $true; $valuesChanged++
                    } catch {
                        Write-Warning "[$($file.Name) / $alias] Grid->BG conversion failed: $_"
                        $valueNode.InnerText = ''
                        $changed = $true; $valuesChanged++
                    }
                }
            }
        } catch {
            Write-Warning "[$($file.Name) / $alias] convert failed: $_"
            $skipped++
        }
    }

    if ($changed -and -not $DryRun) {
        # Preserve CDATA-friendly output by passing through .Save()
        $xml.Save($file.FullName)
        $filesChanged++
    } elseif ($changed -and $DryRun) {
        $filesChanged++
    }
}

Write-Host ""
Write-Host "Files modified:  $filesChanged" -ForegroundColor Green
Write-Host "Values converted: $valuesChanged"
Write-Host "Skipped (errors): $skipped"
if ($DryRun) { Write-Host "DRY RUN -- no files written." -ForegroundColor Yellow }
