$contName = "flane-web-api-product"
$Cont = docker ps --filter "status=running" --filter "name= $contName"  --format '{{ .ID }}'

$ipaddress = docker inspect --format="{{range .NetworkSettings.Networks}}{{.IPAddress}} {{end}}"  $Cont

$ipaddress

Start-Process "chrome.exe" "$ipaddress"