$expectedPercentage = 68.2
dotnet test ./HealthChecks/HealthChecks.sln  /p:CollectCoverage=true /p:CoverletOutput=/TestResults/ /p:MergeWith=/TestResults/coverage.json --logger=trx -r /TestResults/
dotnet test ./Sentinel.Empty.Tests/Sentinel.Empty.Tests.sln /p:CollectCoverage=true /p:MergeWith="/TestResults/coverage.json" /p:CoverletOutputFormat="opencover" /p:CoverletOutput=/TestResults/

dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator "-reports:/TestResults/coverage.opencover.xml" "-targetdir:/TestResults/coveragereport" -reporttypes:"HTMLSummary;TextSummary" -assemblyfilters:"+Sentinel.*;+Mercan.*"
$fileName = "/TestResults/coveragereport/Summary.txt"

$fileExist = Test-Path $fileName -PathType Leaf
If ($fileExist -eq $True) { Write-Host "File Found" }
else {
    Write-Error "Coverage file not Found $fileName"
}
(Get-Content $fileName) | 
Foreach-Object {  
    if ($_ -match " Line coverage") {
        $array = $_.Split(':')
        if ($array.count -eq 2) {
            $currentPercentage = $array[1].Trim().Trim('%')
            if ($currentPercentage -lt $expectedPercentage) {
                Write-Error "Coverage failed Expected : $expectedPercentage% Current : $currentPercentage%"
            }
            else {
                Write-Host "Coverage succeeded Expected : $expectedPercentage% Current : $currentPercentage%"
            }
        }
        else {
            Write-Host "Coverage file Found and read but Line coverage could not retained"
        }
    }
}


