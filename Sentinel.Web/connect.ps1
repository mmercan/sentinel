# $contName = "flane-web-api-product"
# $Cont = docker ps --filter "status=running" --filter "name= $contName"  --format '{{ .ID }}'
# $ipaddress = docker inspect --format="{{range .NetworkSettings.Networks}}{{.IPAddress}} {{end}}"  $Cont
# $ipaddress
# Start-Process "chrome.exe" "$ipaddress"



$productApi = "http://localhost:5003"
$webApiComms = "http://localhost:5004"
$WebSts = "http://localhost:5000"
$webAdmin = "http://localhost:4200"
$dbRedis = "http://localhost:6379"
$dbElasticsearch = "http://localhost:9200/_cat/health"
$dbMongodb = "http://localhost:27017"
$nats = "http://localhost:8222"
$utilMailhog = "http://localhost:8025"
$utilKibana = "http://localhost:5601"
$rabbitmq = "http://localhost:15672/"

Start-Process "chrome.exe" "$productApi"
Start-Process "chrome.exe" "$webApiComms"
Start-Process "chrome.exe" "$WebSts"
Start-Process "chrome.exe" "$webAdmin"
Start-Process "chrome.exe" "$dbRedis"
Start-Process "chrome.exe" "$dbElasticsearch"
Start-Process "chrome.exe" "$dbMongodb"
Start-Process "chrome.exe" "$nats"
Start-Process "chrome.exe" "$utilMailhog"
Start-Process "chrome.exe" "$utilKibana"
Start-Process "chrome.exe" "$rabbitmq"