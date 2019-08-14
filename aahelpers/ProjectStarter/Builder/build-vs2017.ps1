#docker build -t aspcore-full-framework:2.0.5 -f ./docker/Builder/dockerfile .
#docker build --no-cache -t mmercan/aspcore-full-framework:4.6.2 -f ./dockerfile .


$KeyPath = "HKLM:\SYSTEM\CurrentControlSet\Services\vmsmp\parameters\SwitchList"
$keys = get-childitem $KeyPath

foreach ($key in $keys) {
    if ($key.GetValue("FriendlyName") -eq 'nat')	{
        $newKeyPath = $KeyPath + "\" + $key.PSChildName
        Remove-Item -Path $newKeyPath -Recurse
    }
}

remove-netnat -Confirm:$false


docker build -t mmercan/vs2017 -f ./vs2017/dockerfile -m 4GB .