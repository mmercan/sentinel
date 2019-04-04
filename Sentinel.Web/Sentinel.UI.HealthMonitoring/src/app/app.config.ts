import { Injectable } from '@angular/core';


export enum logLevel {
  data = 6,
  debug = 5,
  info = 4,
  log = 3,
  warn = 2,
  error = 1,
  none = 0
}
export enum authenticationType {
  Adal = 1,
  local = 2
}


@Injectable()
export class AppConfig {
  config = {
    name: 'Sentinel Health Monitoring',
    title: 'Sentinel Health Monitoring',
    version: '1.0.0',
    debug: true,
    logLevel: logLevel.debug,
    authenticationType: authenticationType.Adal,
    Nofitication: {
      publicKey: 'BCbYNxjxYPOcv3Hn8xZH1bB2kJLFLeO9Fx68U0C2FOZ7wFmG_yxGdiiNIWrFRHY6X1NL6egRgzZGAC_A_6fcigA',
      subscriptionRepoUrl: window.location.hostname === 'localhost'
        ? 'http://localhost:5000/api/PushNotification' : 'https://decima.azurewebsites.net/api/PushNotification'
    },
    login: {
      loginUrl: window.location.hostname === 'localhost'
        ? 'http://localhost:5000/api/Token' : 'https://decima.azurewebsites.net/api/Token',
      bearerToken: ''
    },
    Api: {
      baseUrl: window.location.hostname === 'localhost'
        ? 'http://localhost:62657/api/' : '/KeyVaultChecker/api/',
      appsettingsFolderLocation: window.location.hostname === 'localhost'
        ? 'C:\\Users\\mmercan\\Desktop\\BAU\\Bupa.Api.BFF.SSP\\Bupa.Api.BFF.SSP.CRM' : 'D:\\home\\site\\wwwroot',
      keyVaultCertThumbPrintLocation: 'KeyVault:CertThumbPrint',
      keyVaultBaseUrlLocation: 'KeyVault:BaseUrl',
      keyVaultClientIdLocation: 'KeyVault:ClientId'
    },
    SignalR: {
      pingServerUrl: window.location.hostname === 'localhost'
        ? 'http://localhost:62657/ping' : '/KeyVaultChecker/ping',
      chatServerUrl: window.location.hostname === 'localhost'
        ? 'http://localhost:62657/hub/chat' : '/KeyVaultChecker/hub/chat',
      notificationHubUrl: window.location.hostname === 'localhost'
        ? 'http://localhost:5000/hub/notifications' : 'KeyVaultChecker/hub/notifications'
    },
    currentUser: {
      username: '',
      emailaddress: '',
      userProfileImageUrl: '',
      userSettings: {}

    },
    adalConfig: {
      tenant: 'e1870496-eab8-42d0-8eb9-75fa94cfc3b8',
      clientId: '67d009b1-97fe-4963-84ff-3590b06df0da',
      redirectUri: window.location.origin + '/',
      postLogoutRedirectUri: window.location.origin + '/',
      cacheLocation: 'localStorage'
    }
  };
  navigatingto: string;
  constructor() { }

  // Public Key
  // BCbYNxjxYPOcv3Hn8xZH1bB2kJLFLeO9Fx68U0C2FOZ7wFmG_yxGdiiNIWrFRHY6X1NL6egRgzZGAC_A_6fcigA
  // Private Key
  // r2HJzuoJiFD0uMDoQcKMQCGo8M80wag8kCoTMFf3S34

}
