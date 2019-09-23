expectedPercentage=32.8
dotnet tool install --global dotnet-reportgenerator-globaltool
dotnet test ./HealthChecks/HealthChecks.sln  /p:CollectCoverage=true /p:CoverletOutput=/TestResults/ /p:MergeWith=/TestResults/coverage.json --logger=trx -r /TestResults/
dotnet test ./Sentinel.Empty.Tests/Sentinel.Empty.Tests.sln /p:CollectCoverage=true /p:MergeWith="/TestResults/coverage.json" /p:CoverletOutputFormat="opencover" /p:CoverletOutput=/TestResults/coverage.opencover.xml
reportgenerator "-reports:/TestResults/coverage.opencover.xml" "-targetdir:/TestResults/coveragereport" -reporttypes:"HTMLSummary;TextSummary" -assemblyfilters:"+Sentinel.*;+Mercan.*"

while read line; do 
if [[ $line == *"Line coverage"* ]]; then
percent=${line##*:}
number=${percent::-1}
echo $number
    if (( $(echo "$number $expectedPercentage" | awk '{print ($1 > $2)}') )); then
        1>&2 echo "Not in expected range Failed Actual Percentage $number Expected Percentage $expectedPercentage"
         
    else
        echo "In expected range Successed Actual Percentage $number Expected Percentage $expectedPercentage"
    fi
fi
done < "/mnt/c/TestResults/coveragereport/Summary.txt"