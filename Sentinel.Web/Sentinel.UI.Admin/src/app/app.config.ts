import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

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
    name: 'Sentinel',
    title: 'Sentinel',
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
        ? 'http://localhost:5000/api/Token' : 'https://auth.myrcan.com/api/Token',
      bearerToken: ''
    },
    Api: {
      productApiBaseUrl: window.location.hostname === 'localhost' ? 'http://localhost:5003/api/' : 'https://product.api.myrcan.com/api/',
      memberApiBaseUrl: window.location.hostname === 'localhost' ? 'http://localhost:5002/api/' : 'https://member.api.myrcan.com/api/',
      commApiBaseUrl: window.location.hostname === 'localhost' ? 'http://localhost:5004/api/' : 'https://comms.api.myrcan.com/api/',
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
