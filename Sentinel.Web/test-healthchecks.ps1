# dotnet test ./HealthChecks/Mercan.HealthChecks.Common.Tests/Mercan.HealthChecks.Common.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/HealthChecks.Common.Tests.xml
# dotnet test ./HealthChecks/Mercan.HealthChecks.Elasticsearch.Tests/Mercan.HealthChecks.Elasticsearch.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/HealthChecks.Elasticsearch.Tests.xml
# dotnet test ./HealthChecks/Mercan.HealthChecks.Mongo.Tests/Mercan.HealthChecks.Mongo.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/HealthChecks.Mongo.Tests.xml
# dotnet test ./HealthChecks/Mercan.HealthChecks.MySql.Tests/Mercan.HealthChecks.MySql.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/HealthChecks.MySql.Tests.xml
# dotnet test ./HealthChecks/Mercan.HealthChecks.Nats.Tests/Mercan.HealthChecks.Nats.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/HealthChecks.Nats.Tests.xml
# dotnet test ./HealthChecks/Mercan.HealthChecks.Network.Tests/Mercan.HealthChecks.Network.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/HealthChecks.Network.Tests.xml
# dotnet test ./HealthChecks/Mercan.HealthChecks.RabbitMQ.Tests/Mercan.HealthChecks.RabbitMQ.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/HealthChecks.RabbitMQ.Tests.xml
# dotnet test ./HealthChecks/Mercan.HealthChecks.Redis.Tests/Mercan.HealthChecks.Redis.Tests.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/TestResults/HealthChecks.Redis.Tests.xml



dotnet test ./HealthChecks/HealthChecks.sln  /p:CollectCoverage=true  /p:CoverletOutput=/TestResults/ /p:MergeWith=/TestResults/coverage.json --logger=trx -r /TestResults/
dotnet test ./Sentinel.Empty.Tests/Sentinel.Empty.Tests.sln /p:CollectCoverage=true /p:MergeWith="/TestResults/coverage.json" /p:CoverletOutputFormat="opencover" /p:CoverletOutput=/TestResults/
# dotnet test ./Sentinel.Empty.Tests/Sentinel.Empty.Tests.sln  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover p:CoverletOutput=/TestResults/ /p:MergeWith=/TestResults/coverage.json