# kubectl create namespace ingress-basic
# helm install stable/nginx-ingress --namespace ingress-basic --set controller.replicaCount=2 --set nodeSelector."beta.kubernetes.io/os"=linux --set rbac.create=false
# kubectl get service -l app=nginx-ingress --namespace ingress-basic
# $IP = "52.247.197.155"
# $DNSNAME = "aks-test-ingress" # Name to associate with public IP address
# # Get the resource-id of the public ip
# $PUBLICIPID = $(az network public-ip list --query "[?ipAddress!=null]|[?contains(ipAddress, '$IP')].[id]" --output tsv)
# $PUBLICIPID
# az network public-ip update --ids $PUBLICIPID --dns-name $DNSNAME # Update public ip address with DNS name

# # helm install stable/cert-manager --namespace kube-system --set ingressShim.defaultIssuerName=letsencrypt-staging --set ingressShim.defaultIssuerKind=ClusterIssuer 
# helm install --name cert-manager01 --namespace kube-system stable/cert-manager --set ingressShim.defaultIssuerName=letsencrypt-prod --set ingressShim.defaultIssuerKind=ClusterIssuer 
# # helm upgrade cert-manager stable/cert-manager --namespace kube-system --set ingressShim.defaultIssuerName=letsencrypt-staging --set ingressShim.defaultIssuerKind=ClusterIssuer
# 

# kubectl get service -l app=nginx-ingress --namespace kube-system
# $PUBLICIPID = $(az network public-ip list --query "[?ipAddress!=null]|[?contains(ipAddress, '$IP')].[id]" --output tsv)
# $PUBLICIPID

##### Run those #####
# helm install --name nginx-ingress stable/nginx-ingress --namespace kube-system --set controller.replicaCount=2 --set nodeSelector."beta.kubernetes.io/os"=linux
# kubectl get service -l app=nginx-ingress --namespace kube-system
# kubectl apply -f https://raw.githubusercontent.com/jetstack/cert-manager/release-0.8/deploy/manifests/00-crds.yaml --namespace kube-system
# helm install --name cert-manager01 --namespace kube-system stable/cert-manager --version v0.5.2 --set ingressShim.defaultIssuerName=letsencrypt-prod --set ingressShim.defaultIssuerKind=ClusterIssuer
# kubectl apply -f cluster-issuer-letsencrypt-prod.yaml


##### Run those #####
# helm install --name nginx-ingress  stable/nginx-ingress --namespace ingress-basic --set controller.replicaCount=2  --set nodeSelector."beta.kubernetes.io/os"=linux
# kubectl get service -l app=nginx-ingress --namespace ingress-basic

# kubectl apply -f https://raw.githubusercontent.com/jetstack/cert-manager/release-0.7/deploy/manifests/00-crds.yaml
# kubectl create namespace cert-manager
# kubectl label namespace cert-manager certmanager.k8s.io/disable-validation=true
# helm repo add jetstack https://charts.jetstack.io
# helm repo update
# helm install --name cert-manager --namespace cert-manager  --version v0.7.0 jetstack/cert-manager
# kubectl apply -f cluster-issuer-letsencrypt-prod.yaml

  