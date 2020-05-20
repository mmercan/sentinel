{
  "sentinel": {
    "dev": {
      "healthChecks": [{
        "name": "HealthMonitoring Api",
        "isaliveandwell": "https://healthmonitoring-api.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Comms Api",
        "isaliveandwell": "https://comms-api.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Member Api",
        "isaliveandwell": "https://member-api.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Product Api",
        "isaliveandwell": "https://product-api.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Product UI",
        "isaliveandwell": "https://product-ui.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "STS UI",
        "isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell"
      }]
    },
    "istio": {
      "healthChecks": [{
        "name": "HealthMonitoring Api",
        "isaliveandwell": "http://healthmonitoring-api.dev.api.sentinel.mercan.io/health/isaliveandwell"
      }, {
        "name": "Comms Api",
        "isaliveandwell": "http://comms-api.dev.api.sentinel.mercan.io/health/isaliveandwell"
      }, {
        "name": "Member Api",
        "isaliveandwell": "http://member-api.dev.api.sentinel.mercan.io/health/isaliveandwell"
      }, {
        "name": "Product Api",
        "isaliveandwell": "http://product-api.dev.api.sentinel.mercan.io/health/isaliveandwell"
      }, {
        "name": "Product UI",
        "isaliveandwell": "http://product-api.dev.api.sentinel.mercan.io/health/isaliveandwell"
      }, {
        "name": "STS UI",
        "isaliveandwell": "http://auth.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Billing",
        "isaliveandwell": "http://billing-api.dev.api.sentinel.mercan.io/health/isaliveandwell"
      }]
    },
    "test": {
      "healthChecks": [{
        "name": "HealthMonitoring Api",
        "isaliveandwell": "https://healthmonitoring-api.dev.myrcan.com/health/isaliveandwell"
      }]
    },
    "euat": {
      "healthChecks": [{
        "name": "HealthMonitoring Api",
        "isaliveandwell": "https://healthmonitoring-api.dev.myrcan.com/health/isaliveandwell"
      }]
    },
    "perf": {
      "healthChecks": [{
        "name": "HealthMonitoring Api",
        "isaliveandwell": "https://healthmonitoring-api.dev.myrcan.com/health/isaliveandwell"
      }]
    }
  },
  "sentient": {
    "dev": {
      "healthChecks": [{
        "name": "HealthMonitoring Api",
        "isaliveandwell": "https://healthmonitoring-api.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Comms Api",
        "isaliveandwell": "https://comms-api.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Member Api",
        "isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Product Api",
        "isaliveandwell": "https://product-api.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "Product UI",
        "isaliveandwell": "https://product-ui.dev.myrcan.com/health/isaliveandwell"
      }, {
        "name": "STS UI",
        "isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell"
      }]
    }
  }
}
