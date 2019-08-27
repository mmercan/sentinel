import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Menu } from '../../menu-items/menu-items';
import { Notification, NotificationService } from '../../notification/notification.service';

@Injectable({
  providedIn: 'root',
})
export class ConfigDataService {

  private dataStore: {
    configData: any;
  };

  public configData: Observable<any>;
  private _configData: BehaviorSubject<any>;
  private storagekey = 'app-configData'
  constructor(private httpClient: HttpClient) {
    this.dataStore = { configData: {} };

    this._configData = new BehaviorSubject([]) as BehaviorSubject<any>;
    this.configData = this._configData.asObservable();
    this.getConfigData();
  }

  getConfigData(): any[] {
    let configstr = localStorage.getItem(this.storagekey);
    if (!configstr) {
      this.httpClient.get('./HealthCheck.json').subscribe((result) => {
        configstr = JSON.stringify(result);
        localStorage.setItem(this.storagekey, configstr);
      });
    }
    if (configstr) {
      const config = JSON.parse(configstr);
      this.dataStore.configData = config;
      this._configData.next(Object.assign({}, this.dataStore).configData);
      return config;
    } else {
      return undefined;
    }
  }

  getMenuItems(): Menu[] {
    const menuItems = [];
    if (Object.entries(this.dataStore.configData).length === 0 && this.dataStore.configData.constructor === Object) {
      this.getConfigData();
    }

    for (const property in this.dataStore.configData) {
      if (this.dataStore.configData.hasOwnProperty(property)) {

        const menuitem = {
          state: 'checks/' + property,
          name: property,
          type: 'sub',
          icon: 'basic-folder',
          children: [],
        };

        for (const subProperty in this.dataStore.configData[property]) {
          if (this.dataStore.configData[property].hasOwnProperty(subProperty)) {
            menuitem.children.push({ state: subProperty, name: subProperty });
          }
        }
        if (menuitem) {
          menuItems.push(menuitem);
        }

      }
    }
    return menuItems;
  }

  getHealthCheckUrls(application: string, environment: string): any {
    let returnvalue;
    if (!application || !environment) {
      return undefined;
    }
    if (Object.entries(this.dataStore.configData).length === 0 && this.dataStore.configData.constructor === Object) {
      this.getConfigData();
    }
    const app = this.dataStore.configData[application];
    if (app) {
      const env = app[environment];
      if (env.healthChecks) {
        returnvalue = env.healthChecks;
      }
    }

    return returnvalue;
  }

  setConfigData(file: File): Observable<string> {
    const obs = Observable.create((observer) => {
      const myReader: FileReader = new FileReader();
      myReader.onloadend = (e) => {
        const content = myReader.result as string;
        const jsoncontent = JSON.parse(content);
        const configstr = JSON.stringify(jsoncontent);
        localStorage.setItem(this.storagekey, configstr);
        this.dataStore.configData = jsoncontent;
        this._configData.next(Object.assign({}, this.dataStore).configData);
        observer.next(configstr);
      };
      myReader.onerror = (evt) => {
        observer.error(evt);
      };
      myReader.readAsText(file);
    });
    return obs;
  }
}
