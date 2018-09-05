Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip($zipfile, $outdir) {
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $archive = [System.IO.Compression.ZipFile]::OpenRead($zipfile)
    foreach ($entry in $archive.Entries) {
        $entryTargetFilePath = [System.IO.Path]::Combine($outdir, $entry.FullName)
        $entryDir = [System.IO.Path]::GetDirectoryName($entryTargetFilePath)
        
        #Ensure the directory of the archive entry exists
        if (!(Test-Path $entryDir )) {
            New-Item -ItemType Directory -Path $entryDir | Out-Null 
        }
        #If the entry is not a directory entry, then extract entry
        if (!$entryTargetFilePath.EndsWith("/")) {
            [System.IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $entryTargetFilePath, $true);
        }
    }
}


$folder = "AWWeb.ng"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path $scriptpath


#$angularFolder = $dir + "\" + $folder

$angularFolder = Join-Path -Path $dir -ChildPath ..\..\..\prototypes\$folder
$angularRootFolder = Join-Path -Path $dir -ChildPath ..\..\..\prototypes
$angularFolder

function new-ng {
    ng new  ClientApp --directory $folder --source-dir ClientApp --routing true --style scss
}
function Add-packages-decima {
    npm install "d3", "@ng-bootstrap/ng-bootstrap", "@swimlane/ngx-charts", "@swimlane/ngx-datatable", "angular-calendar", "angular-tree-component", "angular2-text-mask", "bootstrap@4.0.0", "classlist.js", "d3", "dragula", "intl", "ng-sidebar", "ng2-dragula", "ng2-file-upload" , "ng2-validation", "quill", "resize-observer-polyfill", "screenfull", "spinthatshit", "text-mask-addons", "web-animations-js", "hammerjs", "moment"  --save
    npm install
}
function Add-packages-more {
    npm install "@types/moment", "sw-precache"  --save --dev
    npm install "adal-angular" , "@angular/cdk", "@angular/material", "sw-toolbox" --save  
    # npm install "@types/adal", "@types/adal-angular", "@types/moment", "sw-precache"  --save --dev
    # npm install "adal-angular4 ,@angular/cdk", "@angular/material", "adal-angular", "expose-loader", "sw-toolbox" --save  
}
function Add-SignalR {
    npm install @aspnet/signalr  --save
}
function Add-PWA {
    npm install  "sw-precache" --save --dev
    npm install "sw-toolbox" --save

}
function Add-Styles {

    $styles = @(
        "src/styles.scss",
        "src/assets/fonts/linea/styles.css",
        "src/assets/fonts/header/_flaticon.scss",
        "./node_modules/dragula/dist/dragula.css",
        "./node_modules/quill/dist/quill.snow.css",
        "./node_modules/angular-calendar/scss/angular-calendar.scss",
        "./node_modules/@swimlane/ngx-datatable/release/index.css",
        "src/assets/fonts/data-table/icons.css",
        "src/assets/styles/app.scss",
        "./node_modules/@angular/material/prebuilt-themes/indigo-pink.css"
    )

    $scripts = @( "./node_modules/hammerjs/hammer.min.js",
        "./node_modules/@aspnet/signalr/dist/browser/signalr.min.js"
    )

    $assets = @(
        "src/assets",
        "src/favicon.ico",
        "src/sw.js",
        "src/sw-toolbox.js"
    )

    $jobj = Get-Content '.\angular.json' -raw | ConvertFrom-Json
    $jobj.projects.ClientApp.architect.build.options | ForEach-Object { $_.styles = $styles }
    $jobj.projects.ClientApp.architect.build.options | ForEach-Object { $_.scripts = $scripts }
    $jobj.projects.ClientApp.architect.build.options | ForEach-Object { $_.assets = $assets }

    $jobj.projects.ClientApp.architect.test.options | ForEach-Object { $_.styles = $styles }
    $jobj.projects.ClientApp.architect.test.options | ForEach-Object { $_.scripts = $scripts }
    $jobj.projects.ClientApp.architect.test.options | ForEach-Object { $_.assets = $assets }

    $json = $jobj| ConvertTo-Json -Depth 10
    $json | set-content  '.\angular.json'

}
function Unzip-sources {

    Unzip "$dir\assets.zip" "$angularFolder\src\"
    Unzip "$dir\favicons.zip" "$angularFolder\src\assets\icons\"
    Unzip "$dir\app.zip" "$angularFolder\src\"
}

function Add-ServerRendering {
    $scriptsjson = @"
{
    "build:client-and-server-bundles": "ng build --prod && ng run ClientApp:server",
    "build:prerender": "npm run build:client-and-server-bundles && npm run webpack:server && npm run generate:prerender",
    "build:ssr": "npm run build:client-and-server-bundles && npm run webpack:server",
    "generate:prerender": "cd dist && node prerender",
    "webpack:server": "webpack --config webpack.server.config.js --progress --colors",
    "serve:prerender": "cd dist/browser && http-server",
    "serve:ssr": "node dist/server"
  }
"@

    $scripts = ConvertFrom-Json($scriptsjson)

    $jobj = Get-Content '.\package.json' -raw | ConvertFrom-Json
    $hashtable = @{}
    $scripts.psobject.properties | Foreach { $hashtable[$_.Name] = $_.Value }
    foreach ($h in $hashtable.GetEnumerator()) {
        $jobj.scripts | add-member -Name $h.Name -value $h.Value -MemberType NoteProperty
    }
    $json = $jobj| ConvertTo-Json -Depth 10 | % { [System.Text.RegularExpressions.Regex]::Unescape($_) }
    $json | set-content  '.\package.json'


    $angularserver = @"
{
"builder": "@angular-devkit/build-angular:server",
"options": {
    "outputPath": "dist/server",
    "main": "src/main.server.ts",
    "tsConfig": "src/tsconfig.server.json"
    }
}
"@


    $angularobj = Get-Content '.\angular.json' -raw | ConvertFrom-Json
    $angularobj.projects.ClientApp.architect | add-member -Name "server" -value (Convertfrom-Json $angularserver) -MemberType NoteProperty
    $angularobj.projects.ClientApp.architect.build.options| ForEach-Object { $_.outputPath = "dist/browser" } 

    $json = $angularobj| ConvertTo-Json -Depth 10 | % { [System.Text.RegularExpressions.Regex]::Unescape($_) }

    $json | set-content  '.\angular.json'


    npm install  "@angular/platform-server", "@nguniversal/module-map-ngfactory-loader", "@nguniversal/express-engine", "@nguniversal/express-engine", "ts-loader"
    Unzip "$dir\serverrendering.zip" "$angularFolder\"
}

function Add-WPA {

    $fileName = ".\src\index.html"
    $index = (Get-Content $fileName) | 
        Foreach-Object {
        $_ # send the current line to output
        if ($_ -match "</div>") {

            '<script>'
            'if ("serviceWorker" in navigator) {'
            '  navigator.serviceWorker.register("sw.js").then(function(registration) {'
            '    console.log("Service Worker registered");'
            '  }).catch(function(err) {'
            '    console.log("Service Worker registration failed: ", err);'
            '  });'
            '}'
            '</script>'
            '<noscript>'
            '  Please Enable javascript This Application Requires javascript'
            '</noscript>'
            '  </body>'
            '</html>'
        }
    }

    $index | set-content  $fileName

    #copy ".\ClientApp\index-dev.html" ".\wwwroot\dev"
    #copy ".\ClientApp\index-prod.html" ".\wwwroot\prod"

    Unzip "$dir\wpa.zip" "$angularFolder\src\"

    #  $swprecachedevconfig  | set-content  '.\sw-precache-config.js'
    #  $swprecacheprodconfig | set-content  '.\sw-precache-prod-config.js'
  
    # $jobj = Get-Content '.\package.json' -raw | ConvertFrom-Json
    # $jobj.scripts|add-member -membertype noteproperty -name precache -value "sw-precache --verbose --config=sw-precache-config.js" 
    # $jobj.scripts|add-member -membertype noteproperty -name precacheprod -value "sw-precache --verbose --config=sw-precache-prod-config.js"
    # $json = $jobj| ConvertTo-Json -Depth 5
    # $json | set-content  '.\package.json' 
}


Set-Location -Path $angularRootFolder
new-ng
Set-Location -Path $angularFolder

Unzip-sources
Write-Host "Adding Styles"
Add-Styles
Add-packages-decima
Add-packages-more
Add-PWA
Add-SignalR

Add-WPA

# Add-ServerRendering