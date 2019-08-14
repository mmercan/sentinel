Import-Module .\new-dotnet.ps1 -Force

$folder = "Sentinel.UI.Product"
Write-Host "--------------------------------"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path $scriptpath
$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
$appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder


new-item -type directory -path $appFolder -Force
Write-Host "appFolder: $appFolder"

set-location -Path $appFolder
new-dotnet-newestpackages -port 5005

dotnet add reference ../Mercan/Mercan.Common/Mercan.Common.csproj
dotnet add reference ../Sentinel.Common/Sentinel.Model/Sentinel.Model.csproj

Add-cors-swagger-startupcs
Add-Logger
Add-Api-ConfigureJwtAuthService-startupcs

Add-HeathCheckApi
Add-Dockerfile
Add-watchrunlaunchSettings -port 5005

Add-TestApis

Add-PushNotificationController
#Add-TokenController
Add-SignalR
dotnet restore
dotnet build

$Env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet watch run
#Set-Location $dir

#Add-corsswagger-startupcs

