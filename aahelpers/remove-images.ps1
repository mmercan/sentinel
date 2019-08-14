$imagelist = 'microsoft/dotnet' , 'microsoft/windowsservercore', 'microsoft/mssql-server-windows-developer', 'docker4w/nsenter-dockerd', 'netfx-4.5.2-ssdt', 'mmercan/vs2017'

. ./ConvertFromDocker.ps1

$containers = docker image list   -a --no-trunc | ConvertFrom-Docker

$filteredContainers = $containers | Where-Object {$imagelist -notcontains $_.Repository} 
$filteredContainers |format-table
#$filteredContainersIds = $filteredContainers| Select-Object ImageId

#delete containers before deleting the images
docker rm $(docker ps -a -q)

# delete images
Foreach ($i in $filteredContainers) {
    docker rmi $i.ImageId --force
}
docker image list --all