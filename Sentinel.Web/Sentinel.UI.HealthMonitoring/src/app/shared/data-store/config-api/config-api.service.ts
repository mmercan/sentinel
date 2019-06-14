import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { IConfiguration, IConfigItem } from './interfaces/i-configuration';


import { IEnvironment } from './interfaces/i-environment';
import { ICertificate } from './interfaces/i-certificate';
import { IAppsettingValidate } from './interfaces/i-appsetting-validate';
import { AppConfig } from '../../../app.config';
import { Notification, NotificationService } from '../../notification/notification.service';
@Injectable()
export class ConfigApiService implements OnDestroy {

  public appsettingValidates: Observable<IAppsettingValidate[]>;
  private _appsettingValidates: BehaviorSubject<IAppsettingValidate[]>;

  public environment: Observable<IEnvironment>;
  private _environment: BehaviorSubject<IEnvironment>;

  public certificates: Observable<ICertificate[]>;
  private _certificates: BehaviorSubject<ICertificate[]>;

  public configuration: Observable<any>;
  private _configuration: BehaviorSubject<any>;

  private dataStore: {
    environment: IEnvironment;
    certificates: ICertificate[];
    appsettingValidates: IAppsettingValidate[];
    configuration: IConfiguration;
  };
  getEnvironmentSubscription: Subscription;
  getCertificatesSubscription: Subscription;
  getConfigurationSubscription: Subscription;
  EnvironmentHttpGetSubscription: Subscription;
  getCertificatesHttpPosySubscription: Subscription;
  _getConfigurationSubscription: Subscription;
  httpCallByConfigurationHttpPostSubscription: Subscription;
  _getConfigurationHttpPostSubscription: Subscription;


  constructor(protected http: HttpClient, protected appConfig: AppConfig, protected notificationService: NotificationService) {
    this.dataStore = { environment: {}, certificates: [], appsettingValidates: [], configuration: {} };

    this._environment = <BehaviorSubject<any>>new BehaviorSubject({});
    this.environment = this._environment.asObservable();

    this._certificates = <BehaviorSubject<any>>new BehaviorSubject([]);
    this.certificates = this._certificates.asObservable();

    this._appsettingValidates = <BehaviorSubject<any>>new BehaviorSubject([]);
    this.appsettingValidates = this._appsettingValidates.asObservable();

    this._configuration = <BehaviorSubject<any>>new BehaviorSubject([]);
    this.configuration = this._configuration.asObservable();

    const appsettingsFolderLocation = this.appConfig.config.Api.appsettingsFolderLocation;
    const baseUrl = this.appConfig.config.Api.baseUrl;
    const keyVaultBaseUrlLocation = this.appConfig.config.Api.keyVaultBaseUrlLocation;
    const keyVaultCertThumbPrintLocation = this.appConfig.config.Api.keyVaultCertThumbPrintLocation;
    const keyVaultClientIdLocation = this.appConfig.config.Api.keyVaultClientIdLocation;
    this.getAppsettingValidate();
    this.getEnvironmentSubscription = this.getEnvironment().subscribe(data => { });
    this.getCertificatesSubscription = this.getCertificates(keyVaultCertThumbPrintLocation,
      keyVaultBaseUrlLocation, keyVaultClientIdLocation, appsettingsFolderLocation).subscribe(data => { });


    // this.getConfigurationSubscription = this._getConfiguration(keyVaultCertThumbPrintLocation, keyVaultBaseUrlLocation,
    //   keyVaultClientIdLocation, appsettingsFolderLocation, true).subscribe(data => { });
  }

  getEnvironment(force?: boolean): Observable<IEnvironment> {
    const obs = Observable.create(observer => {
      const baseurl = this.appConfig.config.Api.baseUrl;
      const apiurl = baseurl + 'Environment';

      if (this.dataStore.environment && this.dataStore.environment.environmentName && !force) {
        observer.next(Object.assign({}, this.dataStore).environment);
      } else {
        this.EnvironmentHttpGetSubscription = this.http.get(apiurl).subscribe(data => {
          this.dataStore.environment = data;
          this._environment.next(Object.assign({}, this.dataStore).environment);
          this.notificationService.showVerbose('Environment Varible Load completed', '');

          observer.next(Object.assign({}, this.dataStore).environment);
        }, error => this.handleError(error, observer, 'Environment Varible Load Failed'));
      }
    });
    return obs;
  }

  getCertificates(certThumbPrintLocation: string, keyvaultBaseUrlLocation: string,
    clientIdLocation: string, appsettingsFolderLocation: string, force?: boolean): Observable<ICertificate[]> {
    const obs = Observable.create(observer => {
      const baseurl = this.appConfig.config.Api.baseUrl;
      const apiurl = baseurl + 'Certificates/List';

      if (this.dataStore.certificates && this.dataStore.certificates.length > 0 && !force) {
        observer.next(Object.assign({}, this.dataStore).certificates);
      } else {

        const postbody = JSON.stringify({
          certThumbPrintLocation: certThumbPrintLocation,
          keyvaultBaseUrlLocation: keyvaultBaseUrlLocation,
          clientIdLocation: clientIdLocation,
          appsettingsFolderLocation: appsettingsFolderLocation
        });
        const headers = new HttpHeaders({ 'Content-Type': 'application/json' });


        this.getCertificatesHttpPosySubscription = this.http.post<ICertificate[]>(apiurl, postbody, { headers: headers })
          .subscribe(data => {
            this.dataStore.certificates = data;
            this._certificates.next(Object.assign({}, this.dataStore).certificates);
            this.notificationService.showVerbose('certificates Load completed', '');

            observer.next(Object.assign({}, this.dataStore).certificates);
          }, error => this.handleError(error, observer, 'certificates Load Failed'));
      }
    });
    return obs;
  }
  getConfiguration(): Observable<any> {

    const appsettingsFolderLocation = this.appConfig.config.Api.appsettingsFolderLocation;
    const baseUrl = this.appConfig.config.Api.baseUrl;
    const keyVaultBaseUrlLocation = this.appConfig.config.Api.keyVaultBaseUrlLocation;
    const keyVaultCertThumbPrintLocation = this.appConfig.config.Api.keyVaultCertThumbPrintLocation;
    const keyVaultClientIdLocation = this.appConfig.config.Api.keyVaultClientIdLocation;

    // this._getConfigurationSubscription = this._getConfiguration(
    //   keyVaultCertThumbPrintLocation,
    //   keyVaultBaseUrlLocation,
    //   keyVaultClientIdLocation,
    //   appsettingsFolderLocation)
    //   .subscribe(data => { });

    return this.configuration;
  }


  getAppsettingValidate(): IAppsettingValidate[] {
    const configstr = localStorage.getItem('app-settingsConfig');
    if (configstr) {
      const config = JSON.parse(configstr);
      this.dataStore.appsettingValidates = config;
      this._appsettingValidates.next(Object.assign({}, this.dataStore).appsettingValidates);
      return config;
    } else {
      return undefined;
    }
  }

  setAppsettingValidate(file: File): Observable<string> {
    const obs = Observable.create(observer => {
      const myReader: FileReader = new FileReader();
      myReader.onloadend = (e) => {
        const content = myReader.result.toString();
        const jsoncontent = JSON.parse(content);
        const configstr = JSON.stringify(jsoncontent);
        localStorage.setItem('app-settingsConfig', configstr);
        this.dataStore.appsettingValidates = jsoncontent;
        this._appsettingValidates.next(Object.assign({}, this.dataStore).appsettingValidates);
        observer.next(content);
      };
      myReader.onerror = (evt) => {
        observer.error(evt);
      };
      myReader.readAsText(file);
    });
    return obs;
  }

  httpCallByConfiguration(configNode: string, aliveSuffix: string, aliveAndWellSuffix: string,
    useJWT?: boolean,
    clientId?: string,
    clientSecret?: string
  ) {
    const obs = Observable.create(observer => {
      const baseurl = this.appConfig.config.Api.baseUrl;
      const apiurl = baseurl + 'httpCall/CallByConfiguration';

      const appsettingsFolderLocation = this.appConfig.config.Api.appsettingsFolderLocation;
      const keyVaultBaseUrlLocation = this.appConfig.config.Api.keyVaultBaseUrlLocation;
      const keyVaultCertThumbPrintLocation = this.appConfig.config.Api.keyVaultCertThumbPrintLocation;
      const keyVaultClientIdLocation = this.appConfig.config.Api.keyVaultClientIdLocation;

      const body: any = {
        thumbPrintLocation: keyVaultCertThumbPrintLocation,
        keyVaultBaseUrlLocation: keyVaultBaseUrlLocation,
        clientIdLocation: keyVaultClientIdLocation,
        appsettingsFolderLocation: appsettingsFolderLocation,
        configNode: configNode,
        aliveSuffix: aliveSuffix,
        aliveAndWellSuffix: aliveAndWellSuffix
      };

      if (useJWT && clientId && clientSecret) {
        body.useJWT = useJWT;
        body.clientId = clientId;
        body.clientSecret = clientSecret;
      }

      const postbody = JSON.stringify(body);
      const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

      this.httpCallByConfigurationHttpPostSubscription = this.http.post(apiurl, postbody, { headers: headers })
        .subscribe(data => {
          this.notificationService.showVerbose('CallByConfiguration Load completed', '');
          observer.next(data);
        }, error => this.handleError(error, observer, 'CallByConfiguration Load Failed'));
    });
    return obs;
  }

  // private _getConfiguration(thumbPrintLocation: string, keyVaultBaseUrlLocation: string, clientIdLocation: string,
  //   appsettingsFolderLocation: string, force?: boolean): Observable<any[]> {

  //   const obs = Observable.create(observer => {
  //     const baseurl = this.appConfig.config.Api.baseUrl;
  //     const apiurl = baseurl + 'AppConfiguration/GetConfigurationFromPath';

  //     if (this.dataStore.configuration && this.dataStore.configuration.thumbprintFound && !force) {
  //       observer.next(Object.assign({}, this.dataStore).configuration);
  //       this._configuration.next(Object.assign({}, this.dataStore).configuration);
  //     } else {

  //       const postbody = JSON.stringify({
  //         thumbPrintLocation: thumbPrintLocation,
  //         keyVaultBaseUrlLocation: keyVaultBaseUrlLocation,
  //         clientIdLocation: clientIdLocation,
  //         appsettingsFolderLocation: appsettingsFolderLocation
  //       });
  //       const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

  //       this._getConfigurationHttpPostSubscription = this.http.post<IConfiguration>(apiurl, postbody, { headers: headers })
  //         .subscribe(data => {

  //           let config = {};
  //           if (data && data.activeConfigs && data.activeConfigs.length > 0) {
  //             const activeConfigs: IConfigItem[] = data.activeConfigs;
  //             config = this.ConvertFlattoObject(activeConfigs);
  //             console.log('=================== itemobj starts ====================');
  //             console.log(config);
  //             console.log('=================== itemobj ends ====================');
  //           } else {
  //             this.handleError(data, observer, data.errorMessage);
  //           }
  //           data.activeConfigObject = config;
  //           this.dataStore.configuration = data;
  //           this.notificationService.showVerbose('config Load completed', '');
  //           observer.next(Object.assign({}, this.dataStore).configuration);
  //           this._configuration.next(Object.assign({}, this.dataStore).configuration);

  //           this.matchValidatorstoAppSettings();
  //         }, error => this.handleError(error, observer, 'config Load Failed'));
  //     }
  //   });
  //   return obs;
  // }

  private ConvertFlattoObject(flat: any): any {
    const result = {};
    flat.forEach(item => {
      if (item.value) {
        const parts = item.key.split(':');
        const last = parts.pop();
        let node = result;
        parts.forEach(function (key) {
          node = node[key] = node[key] || {};
        });
        node[last] = item.value;
      }
    });
    return result;
  }

  private handleError(error: any, observer: any, errorMessage: string) {
    console.error('An error occurred', error);
    if (error && error['_body']) { // check response here.
      const message = error.json();
    }
    this.notificationService.showError(errorMessage, <string>error, true);
    observer.error(error.message || error);
  }

  private matchValidatorstoAppSettings() {
    if (this.dataStore.configuration && this.dataStore.configuration.activeConfigs
      && this.dataStore.configuration.activeConfigs.length > 0
      && this.dataStore.appsettingValidates.length > 0
    ) {

      this.dataStore.configuration.activeConfigs.forEach(conf => {

        this.dataStore.appsettingValidates.forEach(val => {
          if (conf.key === val.key) {
            conf.appsettingValidate = val;
          }
        });

      });
    }
  }

  ngOnDestroy(): void {
    if (this.getEnvironmentSubscription) { this.getEnvironmentSubscription.unsubscribe(); }
    if (this.getCertificatesSubscription) { this.getCertificatesSubscription.unsubscribe(); }
    if (this.getConfigurationSubscription) { this.getConfigurationSubscription.unsubscribe(); }
    if (this.EnvironmentHttpGetSubscription) { this.EnvironmentHttpGetSubscription.unsubscribe(); }
    if (this.getCertificatesHttpPosySubscription) { this.getCertificatesHttpPosySubscription.unsubscribe(); }
    if (this._getConfigurationSubscription) { this._getConfigurationSubscription.unsubscribe(); }
    if (this.httpCallByConfigurationHttpPostSubscription) { this.httpCallByConfigurationHttpPostSubscription.unsubscribe(); }
    if (this._getConfigurationHttpPostSubscription) { this._getConfigurationHttpPostSubscription.unsubscribe(); }
  }

}
