Import-Module .\new-dotnet.ps1 -Force

$folder = "Sentinel.Web.Api.Member"
Write-Host "--------------------------------"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path $scriptpath
$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
$appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder


new-item -type directory -path $appFolder -Force
Write-Host "appFolder: $appFolder"

set-location -Path $appFolder
new-dotnet -port 5002
Add-cors-swagger-startupcs
Add-Logger
Add-Api-ConfigureJwtAuthService-startupcs

Add-HeathCheckApi
Add-Dockerfile
Add-watchrunlaunchSettings -port 5002

Add-TestApis

#Add-PushNotificationController
#Add-TokenController
#Add-SignalR
dotnet restore
dotnet build

dotnet add package "Microsoft.Extensions.Caching.Redis"

# dotnet add package "MongoDB.Driver"
# dotnet add package "MongoDB.Driver.Core"
# dotnet add package "MongoDB.Bson"


$Env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet watch run
#Set-Location $dir

#Add-corsswagger-startupcs

