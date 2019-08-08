$resouceGruoup = "akscluster-rg"
$aksName = "aks-matt"
$clientSecret = "<Put the Secret>"
$servicePrincipalGuid = "<Guid Number of AAD app witch can write to Resource Group>"

az aks create -g $resouceGruoup -n  $aksName --service-principal $servicePrincipalGuid --client-secret $clientSecret --location eastus --generate-ssh-keys
# az aks install-cli
# az aks get-credentials -g $resouceGruoup -n $aksName
# kubectl apply -f rbac-virtual-kubelet.yaml
# helm init --service-account tiller --upgrade
# az aks install-connector --resource-group $resouceGruoup --name $aksName --connector-name virtual-kubelet --os-type Both
# kubectl get nodes

