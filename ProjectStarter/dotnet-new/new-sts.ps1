Import-Module .\new-dotnet.ps1 -Force

$folder = "Sentinel.Web.Sts"
Write-Host "--------------------------------"
$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
$appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\\$folder


new-item -type directory -path $appFolder -Force
Write-Host "appFolder: $appFolder"

set-location -Path $appFolder
new-dotnet-Individual
Add-cors-swagger-startupcs
Add-Logger
Add-Sts-startupcs
Add-TokenController

#Add-PushNotificationController
#Add-SignalR

dotnet restore
dotnet build

dotnet ef migrations add tokenDbContext --context TokenDbContext
dotnet ef database update --context TokenDbContext

$Env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet watch run
#Set-Location $dir


