Import-Module .\new-dotnet.ps1 -Force

$folder = "Sentinel.Api.HealthMonitoring"
Write-Host "--------------------------------"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path $scriptpath
$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
$appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder


New-Item -type directory -path $appFolder -Force
Write-Host "appFolder: $appFolder"

Set-Location -Path $appFolder
new-dotnet -port 5006
Add-cors-swagger-startupcs
Add-Logger
Add-Api-ConfigureJwtAuthService-startupcs

Add-HeathCheckApi
Add-Dockerfile
Add-watchrunlaunchSettings -port 5006

Add-TestApis

#Add-PushNotificationController
#Add-TokenController
#Add-SignalR
dotnet.exe restore
dotnet.exe build

dotnet.exe add package "Microsoft.Extensions.Caching.Redis"

# dotnet add package "MongoDB.Driver"
# dotnet add package "MongoDB.Driver.Core"
# dotnet add package "MongoDB.Bson"


$Env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet.exe watch run
#Set-Location $dir

#Add-corsswagger-startupcs

