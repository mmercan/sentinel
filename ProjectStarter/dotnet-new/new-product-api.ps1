Import-Module .\new-dotnet.ps1 -Force

$folder = "Sentinel.Api.Product"
Write-Host "--------------------------------"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path $scriptpath
$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
$appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder
$testFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder".Tests"


# new-item -type directory -path $appFolder -Force
# Write-Host "appFolder: $appFolder"

# set-location -Path $appFolder
# new-dotnet -port 5003
# Add-cors-swagger-startupcs
# Add-Logger
# Add-Api-ConfigureJwtAuthService-startupcs

# Add-HeathCheckApi
# Add-Dockerfile
# Add-watchrunlaunchSettings -port 5003

# Add-TestApis

# #Add-PushNotificationController
# #Add-TokenController
# #Add-SignalR
# dotnet restore
# dotnet build

# dotnet add package "Microsoft.Extensions.Caching.Redis"

# # dotnet add package "MongoDB.Driver"
# # dotnet add package "MongoDB.Driver.Core"
# # dotnet add package "MongoDB.Bson"


# #Set-Location $dir
# #Add-corsswagger-startupcs
# ######

set-location -Path $testFolder

dotnet new xunit
dotnet add package coverlet.msbuild --version 2.0.1
dotnet add package FluentAssertions --version 5.0.0

dotnet add reference ..\$folder\"$folder".csproj

set-location -Path $appRootFolder
dotnet sln add $testFolder

set-location -Path $appFolder
$Env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet watch run


