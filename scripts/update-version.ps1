Set-Location "$PSScriptRoot/.."

$csproj = Get-ChildItem -Filter *.csproj | Select-Object -First 1

if (-not $csproj) {
    Write-Error "No .csproj file found."
    exit 1
}

Write-Host "[INFO] Using project file: $($csproj.Name)"

[xml]$xml = Get-Content $csproj.FullName

# Try to find <PVersion> and <NVersion> regardless of namespace
$pVersionNode = $xml.SelectSingleNode("//PVersion")
$nVersionNode = $xml.SelectSingleNode("//NVersion")

if (-not $pVersionNode -or -not $nVersionNode) {
    Write-Error "Missing <PVersion> or <NVersion> tags."
    exit 2
}

$pVersion = $pVersionNode.InnerText
$nVersion = $nVersionNode.InnerText

Write-Host "[INFO] Current PVersion: $pVersion"
Write-Host "[INFO] Current NVersion: $nVersion"

# Split and bump
$parts = $nVersion -split '\.'
if ($parts.Length -ne 3) {
    Write-Error "NVersion '$nVersion' is not in the expected format (X.Y.Z)"
    exit 3
}

$major = [int]$parts[0]
$minor = [int]$parts[1]
$patch = [int]$parts[2] + 1

$newPVersion = $nVersion
$newNVersion = "$major.$minor.$patch"

# Update values
$pVersionNode.InnerText = $newPVersion
$nVersionNode.InnerText = $newNVersion

Write-Host "[INFO] New PVersion: $newPVersion"
Write-Host "[INFO] New NVersion: $newNVersion"

# Save file
$xml.Save($csproj.FullName)

Write-Host "[SUCCESS] Versions updated in $($csproj.Name)"
