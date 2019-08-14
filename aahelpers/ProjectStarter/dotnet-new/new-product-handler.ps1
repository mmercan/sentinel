Import-Module .\new-dotnet.ps1 -Force
Import-Module .\new-handler.ps1 -Force

$folder = "Sentinel.Handler.Product"
Write-Host "--------------------------------"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path $scriptpath
$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
$appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder

set-location -Path $appFolder

new-consoleApp

update-handler-Programcs

new-item -type directory -path $appFolder\Services -Force
new-item -type directory -path $appFolder\ScheduledTasks -Force

update-handler-ProductAsyncSubscribeService
update-handler-ProductSubscribeService
update-handler-SomeOtherTask
update-handler-QuoteOfTheDayTask
update-handler-appsettings

dotnet build ../Sentinel.Web.sln
dotnet build ../Sentinel.Web.sln
dotnet restore
dotnet build



Add-Dockerfile

# cd $scriptpath
