$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
$apiFolder = Join-Path -Path $dir -ChildPath .\dockapp


# docker-compose -f docker-compose-prod-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-web-sts sentinel-api-product sentinel-db-redis sentinel-db-mongodb sentinel-services-nats sentinel-api-comms
# docker-compose -f docker-compose-prod-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-web-sts sentinel-web-admin
# docker-compose -f docker-compose-prod-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-web-admin

docker-compose -f docker-compose-prod-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-db-redis sentinel-db-mongodb sentinel-services-nats sentinel-web-sts sentinel-api-product sentinel-api-comms  sentinel-web-admin


