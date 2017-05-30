$ErrorActionPreference = "Stop"

$assemblyVersion = "1.0.0"
$assemblyFileVersion = "1.0.0.17150"
$assemblyInformationalVersion = "1.0.0-alpha03"
$nugetsDestinationPath = "..\nuget-builds\$($assemblyInformationalVersion)"

Write-Host @"

Packing all solution as NuGets:
    AssemblyVersion: $($assemblyVersion)
    AssemblyFileVersion: $($assemblyFileVersion)
    AssemblyInformationalVersion: $($assemblyInformationalVersion)
    NuGetsDestinationFolder: $($nugetsDestinationPath)

"@

Write-Host "Making a major cleanup..."

if(Test-Path $nugetsDestinationPath){
    Remove-Item $nugetsDestinationPath -Recurse
}
New-Item $nugetsDestinationPath -ItemType Directory

Get-ChildItem -Path ".." -Recurse | 
Where-Object {$_.Name -eq "bin" -or $_.Name -eq "obj" -or $_.Name -eq "project.lock.json"} |
ForEach-Object{
    Write-Host "Deleting $($_.FullName)..."
    Remove-Item $_.FullName -Recurse
}

$xprojFiles = Get-ChildItem -Path ".." -Recurse | Where-Object {$_.Name -like "*.xproj"}

Write-Host "Restoring all packages..."
$xprojFiles | ForEach-Object  {
    Write-Host "Restoring packages for $($_.FullName)..."
    dotnet.exe restore $_.DirectoryName --no-cache
}

Write-Host "Building all projects..."
$xprojFiles | ForEach-Object  {
    Write-Host "Building project $($_.FullName)..."
    dotnet.exe build $_.DirectoryName -c Release
}

Write-Host "Running all tests..."
$xprojFiles | Where-Object {$_.FullName -like "*test*"} | ForEach-Object  {
    Write-Host "Tunning tests from $($_.FullName)..."
    dotnet.exe test $_.DirectoryName
}

Write-Host "Packing all NuGets..."

$xprojFiles | Where-Object {$_.FullName -like "*src*"} | ForEach-Object  {
    Write-Host "Packing NuGet of $($_.FullName)..."
    dotnet.exe pack $_.DirectoryName -c Release -o $($nugetsDestinationPath)
}
