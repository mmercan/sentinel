expectedPercentage=20.9
# dotnet tool install --global dotnet-reportgenerator-globaltool
# dotnet test ./HealthChecks/HealthChecks.sln  /p:CollectCoverage=true /p:CoverletOutput=/mnt/c/TestResults/ /p:MergeWith=/mnt/c/TestResults/coverage.json --logger=trx -r /mnt/c/TestResults/
#dotnet test ./Sentinel.Empty.Tests/Sentinel.Empty.Tests.sln /p:CollectCoverage=true /p:MergeWith="/mnt/c/TestResults/coverage.json" /p:CoverletOutputFormat="opencover" /p:CoverletOutput=/mnt/c/TestResults/coverage.opencover.xml
# reportgenerator "-reports:/mnt/c/TestResults/coverage.opencover.xml" "-targetdir:/mnt/c/TestResults/coveragereport" -reporttypes:"HTMLSummary;TextSummary" -assemblyfilters:"+Sentinel.*;+Mercan.*"

while read line; do 
if [[ $line == *"Line coverage"* ]]; then
percent=${line##*:}
number=${percent::-1}
# number=73.0
echo $number
    if (( $(echo "$number $expectedPercentage" | awk '{print ($1 > $2)}') )); then
        echo "In expected range Successed Actual Percentage $number Expected Percentage $expectedPercentage"
    else
    1>&2 echo "Not in expected range Failed Actual Percentage $number Expected Percentage $expectedPercentage"
    fi
fi
done < "/mnt/c/TestResults/coveragereport/Summary.txt"