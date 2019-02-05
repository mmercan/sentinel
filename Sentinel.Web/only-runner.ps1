$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
$apiFolder = Join-Path -Path $dir -ChildPath .\dockapp

# docker run --rm -it microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 dotnet
# docker run --rm -it microsoft/dotnet:2.1-sdk-nanoserver-1709 dotnet
# docker run --rm -it -p 5000:5000 -v $apiFolder\dockapp:C:\app -w C:\app microsoft/dotnet:2.1-sdk-nanoserver-1709 dotnet watch run
#COMPOSE_CONVERT_WINDOWS_PATHS 0



# new-item -type directory -path $appFolder -Force
# Write-Host "appFolder: $appFolder"

#set-location -Path $dir
#docker build  -f ./dotnetrun-dockerfile -t dockapp:V3 .

# set-location -Path $dir
# docker run --rm -it -p 5000:5000 -v "C:\repos\sentinel\Sentinel.Web\dockerplay\dockapp:C:\app" -w "C:\app\myapp"  dockapp:V3  dotnet watch run
# docker run --rm -it -p 5000:5000 -v "C:\repos\sentinel\Sentinel.Web\dockerplay\dockapp:C:\app" -w "C:\app\myapp"  dockapp:V3  cmd
# docker run --rm -it -p 8000:80 -v C:\repos\sentinel\Sentinel.Web\dockerplay\dockapp:C:\app\ -w /app/aspnetapp microsoft/dotnet:2.1-sdk dotnet watch run
# docker run --rm -it -p 5000:5000 -v C:\repos\sentinel\Sentinel.Web\dockerplay\dockapp:c:\app\ -w \app\myapp --name aspnetappsample microsoft/dotnet:2.1-sdk  dotnet watch run

# '--platform=linux
# docker rm $(docker ps -a -q)

# docker-compose -f docker-compose.develop.yml up --build

# docker exec -ti <container name> /bin/bash
#docker exec -ti sentinelweb_test3app-develop51_1  /bin/bash
#3bbac63951b5


#docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-api-product
#docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sqldb
# npm rebuild node-sass --force
# docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-ui-admin sentinel-ui-sts sentinel-api-product
#docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-ui-admin


#docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-ui-sts sentinel-api-product sentinel-ui-admin
#docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-api-comms sentinel-elk-proxy

#docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-util-zookeeper sentinel-util-kafka sentinel-util-kafka-manager

# sentinel-util-kibana  sentinel-db-elasticsearch sentinel-api-member sentinel-util-zookeeper sentinel-util-kafka sentinel-util-kafka-manager


# docker-compose --verbose
#docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-api-product sentinel-ui-sts

# docker-compose -f docker-compose-debug-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-util-mailhog sentinel-db-mongodb  sentinel-db-redis sentinel-ui-sts sentinel-api-product sentinel-api-comms sentinel-handler-comms sentinel-db-elasticsearch sentinel-util-kibana
docker-compose -f docker-compose-debug-linux.yml up sentinel-util-mailhog sentinel-db-mongodb sentinel-service-nats sentinel-db-redis sentinel-ui-sts sentinel-api-product sentinel-api-comms sentinel-handler-comms sentinel-handler-product sentinel-db-elasticsearch sentinel-util-kibana sentinel-service-rabbitmq sentinel-db-mysql sentinel-ui-wordpress
