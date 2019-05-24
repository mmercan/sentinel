import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonDataStoreService } from '../common-data-store/common-data-store.service';
import { HealthReport, HealthReportEntry, HealthReportUrl } from './interfaces/health-report';
import { AppConfig } from '../../../app.config';
import { CommonDataStoreInterface } from '../common-data-store/common-data-store-interface';
import { AuthService } from '../../authentication/auth.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class HealthcheckDataStoreSetService {

  public dataset: Observable<HealthReport[]>;
  private _dataset: BehaviorSubject<HealthReport[]>;
  private dataStore: {
    dataset: HealthReport[];
  };

  constructor(private http: HttpClient, private authService: AuthService, private appConfig: AppConfig) {
    this.dataStore = { dataset: [] };
    this._dataset = <BehaviorSubject<HealthReport[]>>new BehaviorSubject([]);
    this.dataset = this._dataset.asObservable();
  }

  AddHealthCheck(baseUrl: string, servicename: string) {

    const init: HealthReport = { status: 'Loading', servicename: servicename, duration: '0', results: [] };
    this.addtothelist(init, baseUrl, servicename);

    this.http.get<HealthReport>(baseUrl).pipe(map(response => response)).subscribe(data => {
      this.addtothelist(data, baseUrl, servicename);
      console.log('Loading completed');
    }, error => {
      if (error.status === 503) {
        console.log('Could not load items.' + JSON.stringify(error));
        this.addtothelist(error.error, baseUrl, servicename);
      } else {
        const data: HealthReport = {
          status: 'Unhealthy', servicename: servicename, duration: '0', results: [
            {
              type: 'IsAliveRequestFailed', description: '', duration: '0', status: 'Unhealthy',
              name: 'Request Failed', data: { url: baseUrl }, exception: null
            }
          ]
        };
        this.addtothelist(data, baseUrl, servicename);
      }
    });
  }

  GetHealthCheckUrls(application: string, environment: string): Observable<HealthReportUrl[]> {
    const obs = Observable.create(observer => {
      const data = this.appConfig.config.HealthCheck.urls;
      observer.next(data);
    });
    return obs;
  }

  private handleError(error: any, observer: any, errorMessage: string) {
    console.error('An error occurred', error);
    if (error && error['_body']) { // check response here.
      const message = error.json();
    }
    // this.notificationService.showError(errorMessage, <string>error, true);
    observer.error(error.message || error);
  }


  private addtothelist(data: HealthReport, baseUrl: string, servicename: string) {
    data.url = baseUrl;
    data.servicename = servicename;
    if (this.dataStore.dataset && this.dataStore.dataset.length !== undefined) {
      const item = this.dataStore.dataset.find(x => x.url === baseUrl);
      if (item) {
        const itemIndex = this.dataStore.dataset.findIndex(x => x.url === baseUrl);
        if (itemIndex !== undefined) {
          this.dataStore.dataset[itemIndex] = data;
        }
      } else {
        this.dataStore.dataset.push(data);
      }
      // debugger;
      this._dataset.next(Object.assign({}, this.dataStore).dataset);
    }
  }
}
