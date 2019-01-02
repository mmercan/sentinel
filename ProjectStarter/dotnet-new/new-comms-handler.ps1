$folder = "Sentinel.Handler.Comms"
Write-Host "--------------------------------"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path $scriptpath
$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
$appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder

new-item -type directory -path $appFolder -Force
Write-Host "appFolder: $appFolder"
set-location -Path $appFolder


dotnet new console
dotnet add package "NATS.Client" -v 0.8.1
dotnet add package "STAN.Client" -v 0.1.4
dotnet add package "Microsoft.AspNetCore.App" -v 2.1.2

dotnet add package "Microsoft.Extensions.Configuration"



