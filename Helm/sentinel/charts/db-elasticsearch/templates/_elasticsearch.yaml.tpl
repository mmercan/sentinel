cluster:
  name: ${CLUSTER_NAME}

network:
  host: "0.0.0.0"

discovery:
  zen:
    minimum_master_nodes: 1

xpack:
  monitoring:
    enabled: true