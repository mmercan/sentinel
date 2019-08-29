$aspnetfolderPath = "Sentinel.Api.HealthMonitoring"
$ngfolderPath = "Sentinel.UI.HealthMonitoring"
$extensionPath = "Sentinel.Extension.HealthMonitoring"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path -parent $scriptPath # Split-Path $scriptpath 
$parentfolder = Split-Path -parent $dir
$aspnetfolder = $parentfolder + "\" + $aspnetfolderPath
$extensionFolder = $parentfolder + "\" + $extensionPath
$ngfolder = $parentfolder + "\" + $ngfolderPath

$aspnetfolder
Set-Location -Path $aspnetfolder
dotnet publish --output $extensionFolder/artifacts/ -f netcoreapp2.2 -c Release

Copy-Item $extensionFolder/applicationHost.xdt $extensionFolder/artifacts
 
$ngfolder
Set-Location -Path $ngfolder
ng build --prod --output-path "$extensionFolder/artifacts/wwwroot" --base-href "" --aot --extract-css --vendor-chunk

Set-Location -Path $dir
# Set-Location -Path "$extensionFolder/artifacts"
# dotnet .\Sentinel.Api.HealthMonitoring.dll
./nuget pack -NoPackageAnalysis
$nupkgfilename = @(Get-Childitem -path ./* -Include SentinelHealthMonitoring* -exclude *.nuspec)[0].Name
# # dotnet nuget push $nupkgfilename -k <key> -s https://api.nuget.org/v3/index.json
$nupkgfilename
dotnet nuget push $nupkgfilename -k $env:mygetKey -s https://www.myget.org/F/mmercan/api/v3/index.json
Move-Item SentinelHealthMonitoring*.nupkg ./outputs -Force
#Set-Location -Path $dir