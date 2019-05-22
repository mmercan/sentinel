#helm install --name nginx-ingress stable/nginx-ingress --namespace kube-system --set controller.replicaCount=2

# helm install stable/cert-manager --namespace kube-system --set ingressShim.defaultIssuerName=letsencrypt-staging --set ingressShim.defaultIssuerKind=ClusterIssuer 
#helm install --name cert-manager01 --namespace kube-system stable/cert-manager --set ingressShim.defaultIssuerName=letsencrypt-prod --set ingressShim.defaultIssuerKind=ClusterIssuer 
#       helm upgrade cert-manager stable/cert-manager --namespace kube-system --set ingressShim.defaultIssuerName=letsencrypt-staging --set ingressShim.defaultIssuerKind=ClusterIssuer

#kubectl apply -f cluster-issuer-letsencrypt-prod.yaml


kubectl apply -f sentinel-admin-cert-ingress-prod.yaml
# kubectl apply -f sentinel-auth-cert-ingress-prod.yaml
# kubectl apply -f sentinel-comms-cert-ingress-prod.yaml
# kubectl apply -f sentinel-kibana-cert-ingress-prod.yaml
# kubectl apply -f sentinel-member-cert-ingress-prod.yaml
# kubectl apply -f sentinel-product-cert-ingress-prod.yaml

