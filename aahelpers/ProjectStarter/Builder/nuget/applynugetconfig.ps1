$pat = "__pass__"
$ConfigFile = "./nuget.config"
$username = "mmercan"


./Nuget.exe sources  Add -Name "NuGet" -Source "https://api.nuget.org/v3/index.json"  -ConfigFile $ConfigFile

./Nuget.exe sources  Add -Name "Api.Framework" -Source "https://mmercan.pkgs.visualstudio.com/_packaging/Api.Framework/nuget/v3/index.json" -ConfigFile $ConfigFile -username $username -password $pat -StorePasswordInClearText
