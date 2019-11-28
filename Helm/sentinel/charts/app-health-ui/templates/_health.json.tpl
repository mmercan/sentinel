{
  "name": "Sentinel Health Monitoring",
  "title": "Sentinel Health Monitoring",
  "version": "1.0.0",
  "debug": true,
  "logLevel": "logLevel.debug",
  "authenticationType": "authenticationType.Adal",
  "Nofitication": {
    "publicKey": "BCbYNxjxYPOcv3Hn8xZH1bB2kJLFLeO9Fx68U0C2FOZ7wFmG_yxGdiiNIWrFRHY6X1NL6egRgzZGAC_A_6fcigA",
    "subscriptionRepoUrl": "https://decima.azurewebsites.net/api/PushNotification"
  },
  "login": {
    "loginUrl": "https://decima.azurewebsites.net/api/Token",
    "bearerToken": ""
  },
  "HealthCheck": {
    "urls": [{
      "name": "HealthMonitoring Api",
      "isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell",
    }, {
      "name": "Comms Api",
      "isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell",
    }, {
      "name": "Member Api",
      "isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell",
    }, {
      "name": "Product Api",
      "isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell",
    }, {
      "name": "Product UI",
      "isaliveandwell": "https://product.myrcan.com/health/isaliveandwell",
    }, {
      "name": "STS UI",
      "isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell",
    }],
    "baseUrl": "https://healthmonitoring.myrcan.com/api/"
  }
}