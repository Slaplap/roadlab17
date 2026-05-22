<#
.SYNOPSIS
    Unwraps Tiptap-format branchMap values in uSync content configs so the
    field can be switched from Umbraco.RichText to Umbraco.TextArea without
    editors seeing raw JSON.

.DESCRIPTION
    Tiptap (the v17 RTE) stores values as {"blocks": {...}, "markup": "..."}.
    We're switching the branchMap data type to TextArea, which renders the
    raw stored value verbatim. This script walks every uSync v17 content
    config that has a <branchMap><Value>...</Value></branchMap> block,
    deserialises the JSON envelope, and replaces the value with just the
    `markup` string (the actual iframe HTML).

    Only modifies <branchMap> values that look like the Tiptap envelope.
    Leaves values that are already plain HTML alone (idempotent).

.PARAMETER ContentFolder
    Directory containing the uSync content config files.
#>
[CmdletBinding()]
param(
    [string]$ContentFolder = "C:\dev\Roadlab\Interon.Roadlab.Web.v17\uSync\v17\Content"
)

$ErrorActionPreference = 'Stop'

$files = Get-ChildItem -Path $ContentFolder -Filter '*.config' -File
$changed = 0
$skipped = 0
foreach ($file in $files) {
    [xml]$xml = Get-Content -LiteralPath $file.FullName -Raw -Encoding UTF8
    if ($xml.DocumentElement.LocalName -eq 'Empty') { continue }
    $node = $xml.SelectSingleNode('//Properties/branchMap/Value')
    if ($null -eq $node) { continue }
    $raw = $node.InnerText
    if ([string]::IsNullOrWhiteSpace($raw)) { continue }
    $trim = $raw.TrimStart()
    if (-not $trim.StartsWith('{')) { $skipped++; continue }   # already plain
    try {
        $envelope = $raw | ConvertFrom-Json
    } catch {
        Write-Warning "[$($file.Name)] branchMap is not valid JSON; skipping"
        continue
    }
    if (-not $envelope.PSObject.Properties.Match('markup').Count) { $skipped++; continue }
    $markup = [string]$envelope.markup
    $node.InnerText = $markup
    $xml.Save($file.FullName)
    Write-Host "  unwrapped $($file.Name)"
    $changed++
}
Write-Host ""
Write-Host "Files updated: $changed"
Write-Host "Already plain or N/A: $skipped"
