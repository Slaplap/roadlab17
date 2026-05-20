<#
.SYNOPSIS
    Converts v13-era property data (Nested Content, MediaPicker v1) in
    umbracoPropertyData to v17 format (Block List, MediaPicker3).

.DESCRIPTION
    After v13->v17 schema migration via uSync, content property values are
    still in v13 storage format and unreadable by the new editors. This
    script transforms them in-place.

    Transformations performed:
      Nested Content JSON  -> Block List JSON
      MediaPicker v1 UDI(s) -> MediaPicker3 JSON array

    Grid -> Block Grid is NOT handled (separate problem, single property,
    handle manually or in a follow-up). The script logs Grid rows it skipped.

    Safe to run multiple times - already-converted rows are detected and
    skipped (BL data starts with '{', MP3 data starts with '[').

    Runs against the connection string in -ConnectionString. Default is
    the local LocalDB instance.

.PARAMETER ConnectionString
    SQL Server connection string. Defaults to the local LocalDB used by
    appsettings.Development.json.

.PARAMETER DryRun
    If set, calculates conversions and prints what would change, but does
    not write to the database.

.EXAMPLE
    .\Convert-V13-To-V17-Data.ps1
    Runs against LocalDB Roadlab_Local and applies conversions.

.EXAMPLE
    .\Convert-V13-To-V17-Data.ps1 -DryRun
    Shows what would be converted without changing anything.
#>
[CmdletBinding()]
param(
    [string]$ConnectionString = "Server=(localdb)\MSSQLLocalDB;Database=Roadlab_Local;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=True;",
    [switch]$DryRun
)

Add-Type -AssemblyName System.Data
$ErrorActionPreference = 'Stop'

function Get-UdiFromKey([string]$key) {
    return "umb://element/" + ($key -replace '-','')
}

function ConvertTo-Mp3Item([string]$udi) {
    if ([string]::IsNullOrWhiteSpace($udi)) { return $null }
    if ($udi -notmatch 'umb://media/([0-9a-fA-F]{32}|[0-9a-fA-F\-]{36})') { return $null }
    $raw = $matches[1] -replace '-',''
    $mediaKey = "$($raw.Substring(0,8))-$($raw.Substring(8,4))-$($raw.Substring(12,4))-$($raw.Substring(16,4))-$($raw.Substring(20,12))"
    return [pscustomobject]@{
        key = [guid]::NewGuid().ToString()
        mediaKey = $mediaKey
    }
}

$conn = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)
$conn.Open()
Write-Host "Connected to $($conn.Database)" -ForegroundColor Green

# 1) Build alias -> contentTypeKey map for element types
$aliasToKey = @{}
$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
SELECT n.uniqueId, ct.alias
FROM umbracoNode n
INNER JOIN cmsContentType ct ON n.id = ct.nodeId
WHERE n.nodeObjectType = 'A2CB7800-F571-4787-9638-BC48539A0EFB'
"@
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    $aliasToKey[$reader.GetString(1)] = $reader.GetGuid(0).ToString()
}
$reader.Close()
Write-Host "Loaded $($aliasToKey.Count) content type alias -> key mappings"

# 2) Find rows needing conversion
$cmd.CommandText = @"
SELECT pd.id, dt.propertyEditorAlias, pt.Alias AS propertyAlias,
       pd.textValue, pd.varcharValue
FROM umbracoPropertyData pd
INNER JOIN cmsPropertyType pt ON pd.propertyTypeId = pt.id
INNER JOIN umbracoDataType dt ON pt.dataTypeId = dt.nodeId
WHERE dt.propertyEditorAlias IN ('Umbraco.BlockList', 'Umbraco.MediaPicker3', 'Umbraco.BlockGrid')
  AND COALESCE(pd.textValue, pd.varcharValue) IS NOT NULL
  AND LTRIM(COALESCE(pd.textValue, pd.varcharValue)) <> ''
"@
$rows = @()
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    $rows += [pscustomobject]@{
        Id           = $reader.GetInt32(0)
        Editor       = $reader.GetString(1)
        PropAlias    = $reader.GetString(2)
        TextValue    = if (-not $reader.IsDBNull(3)) { $reader.GetString(3) } else { $null }
        VarcharValue = if (-not $reader.IsDBNull(4)) { $reader.GetString(4) } else { $null }
    }
}
$reader.Close()
Write-Host "Inspecting $($rows.Count) candidate rows"

# 3) Process each row
$updates = @()
$gridSkipped = 0
$alreadyV17 = 0
$badData = 0

foreach ($r in $rows) {
    $orig = if ($r.TextValue) { $r.TextValue } else { $r.VarcharValue }
    $col = if ($r.TextValue) { 'textValue' } else { 'varcharValue' }
    $trimmed = $orig.TrimStart()

    if ($r.Editor -eq 'Umbraco.BlockList') {
        if ($trimmed.StartsWith('{')) { $alreadyV17++; continue }
        if (-not $trimmed.StartsWith('[')) { $badData++; continue }
        try {
            $ncItems = $orig | ConvertFrom-Json
            $layout = @()
            $contentData = @()
            foreach ($item in $ncItems) {
                $alias = $item.ncContentTypeAlias
                if (-not $aliasToKey.ContainsKey($alias)) {
                    Write-Warning "Row $($r.Id) ($($r.PropAlias)): unknown element type alias '$alias'"
                    continue
                }
                $udi = Get-UdiFromKey $item.key
                $layout += [pscustomobject]@{ contentUdi = $udi }
                $data = [ordered]@{
                    contentTypeKey = $aliasToKey[$alias]
                    udi            = $udi
                }
                foreach ($prop in $item.PSObject.Properties) {
                    if ($prop.Name -in @('key','name','ncContentTypeAlias')) { continue }
                    $data[$prop.Name] = $prop.Value
                }
                $contentData += [pscustomobject]$data
            }
            $bl = [ordered]@{
                layout       = [ordered]@{ "Umbraco.BlockList" = $layout }
                contentData  = $contentData
                settingsData = @()
            }
            $newJson = $bl | ConvertTo-Json -Depth 20 -Compress
            $updates += [pscustomobject]@{ Id = $r.Id; Col = $col; NewValue = $newJson; Kind = 'NC->BL'; PropAlias = $r.PropAlias }
        } catch {
            Write-Warning "Row $($r.Id) ($($r.PropAlias)) NC->BL parse failed: $_"
            $badData++
        }
    }
    elseif ($r.Editor -eq 'Umbraco.MediaPicker3') {
        if ($trimmed.StartsWith('[')) { $alreadyV17++; continue }
        try {
            $items = @()
            foreach ($u in ($orig -split ',')) {
                $mp = ConvertTo-Mp3Item $u.Trim()
                if ($mp) { $items += $mp }
            }
            if ($items.Count -eq 0) { $badData++; continue }
            $newJson = $items | ConvertTo-Json -Depth 5 -Compress
            if ($newJson -notmatch '^\[') { $newJson = "[$newJson]" }  # single-item arrays
            $updates += [pscustomobject]@{ Id = $r.Id; Col = $col; NewValue = $newJson; Kind = 'MP1->MP3'; PropAlias = $r.PropAlias }
        } catch {
            Write-Warning "Row $($r.Id) ($($r.PropAlias)) MP1->MP3 parse failed: $_"
            $badData++
        }
    }
    elseif ($r.Editor -eq 'Umbraco.BlockGrid') {
        if ($trimmed.StartsWith('{') -and $trimmed -notmatch '"sections"') { $alreadyV17++; continue }
        $gridSkipped++
        # Grid->BG is not handled here. Log only.
    }
}

Write-Host ""
Write-Host "Summary:"
Write-Host "  NC->BL conversions:    $($updates | Where-Object Kind -eq 'NC->BL' | Measure-Object | Select-Object -ExpandProperty Count)"
Write-Host "  MP1->MP3 conversions:  $($updates | Where-Object Kind -eq 'MP1->MP3' | Measure-Object | Select-Object -ExpandProperty Count)"
Write-Host "  Grid rows skipped:     $gridSkipped"
Write-Host "  Already v17:           $alreadyV17"
Write-Host "  Bad data (skipped):    $badData"

if ($DryRun) {
    Write-Host ""
    Write-Host "DRY RUN -- no changes written." -ForegroundColor Yellow
    $conn.Close()
    return
}

# 4) Write updates
$tx = $conn.BeginTransaction()
try {
    foreach ($u in $updates) {
        $up = $conn.CreateCommand()
        $up.Transaction = $tx
        $up.CommandText = "UPDATE umbracoPropertyData SET $($u.Col) = @v WHERE id = @id"
        [void]$up.Parameters.AddWithValue("@v", $u.NewValue)
        [void]$up.Parameters.AddWithValue("@id", $u.Id)
        [void]$up.ExecuteNonQuery()
    }
    $tx.Commit()
    Write-Host ""
    Write-Host "Committed $($updates.Count) updates." -ForegroundColor Green
} catch {
    $tx.Rollback()
    Write-Error "Rolled back. Error: $_"
}

$conn.Close()
