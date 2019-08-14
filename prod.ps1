$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
$apiFolder = Join-Path -Path $dir -ChildPath .\dockapp


# docker-compose -f docker-compose-prod-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-ui-sts sentinel-api-product sentinel-db-redis sentinel-db-mongodb sentinel-service-nats sentinel-api-comms
# docker-compose -f docker-compose-prod-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-ui-sts sentinel-ui-admin
# docker-compose -f docker-compose-prod-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-ui-admin

docker-compose -f docker-compose-prod-linux.yml up --build --force-recreate --renew-anon-volumes sentinel-db-redis sentinel-db-mongodb sentinel-service-nats sentinel-ui-sts sentinel-api-product sentinel-api-comms  sentinel-ui-admin


