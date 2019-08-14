
 $contName = "sentinel-api-product"
 $Cont = docker ps --filter "status=running" --filter "name= $contName"  --format '{{ .ID }}'

#$Cont= 'sentinel-api-product'
# $ip = docker inspect --format="{{range .NetworkSettings.Networks}}{{.IPAddress}} {{end}}"  $Cont
# $ip
cls
$Cont
docker logs --since 30s -f  $Cont