#docker-compose.exe -f docker-compose-debug-linux_secrets.yml -f docker-compose-debug-linux.yml build  sentinel-api-billing sentinel-api-scheduler sentinel-api-shipping
# sentinel-api-comms-build


#docker build -f ./Sentinel.Api.Product/dockerfile-linux-test --target test C:\repos\sentinel\Sentinel.Web

#docker build -f ./Sentinel.Api.HealthMonitoring/dockerfile-linux-test --target test --build-arg buildtime_APPID=67d009b1-97fe-4963-84ff-3590b06df0da --build-arg buildtime_APPSECRET=mvcwhjfpDRQT4FYz2Hzg9xOcUchbtr1xWIXZbMjLLps= --build-arg buildtime_ADID=e1870496-eab8-42d0-8eb9-75fa94cfc3b8 C:\repos\sentinel\Sentinel.Web

# gg

#dotnet test ./Sentinel.Api.Product/Sentinel.Api.Product.sln /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/
#dotnet test ./Sentinel.Api.HealthMonitoring/Sentinel.Api.HealthMonitoring.sln /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/

#dotnet test ./Sentinel.Api.Comms/Sentinel.Api.Comms.sln /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/






# dotnet test ./HealthChecks/Mercan.HealthChecks.Common.Tests/Mercan.HealthChecks.Common.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/Mercan.HealthChecks.Common.Tests.xml
# dotnet test ./Sentinel.Api.Comms.Tests/Sentinel.Api.Comms.Tests.csproj /p:CollectCoverage=true  /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/Sentinel.Api.Comms.Tests.xml 


dotnet test ./Sentinel.Api.Comms/Sentinel.Api.Comms.sln -l trx -r /results /p:CollectCoverage=true /p:CoverletOutputFormat=opencover  /p:CoverletOutput=/results/coverage