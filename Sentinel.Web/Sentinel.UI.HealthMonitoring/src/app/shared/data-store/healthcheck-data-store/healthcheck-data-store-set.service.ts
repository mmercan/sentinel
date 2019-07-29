import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../../authentication/auth.service';
import { CommonDataStoreInterface } from '../common-data-store/common-data-store-interface';
import { CommonDataStoreService } from '../common-data-store/common-data-store.service';
import { ConfigDataService } from '../config-data/config-data.service';
import { HealthReport, HealthReportEntry, HealthReportUrl } from './interfaces/health-report';

@Injectable({
  providedIn: 'root',
})
export class HealthcheckDataStoreSetService implements OnDestroy {
  public dataset: Observable<HealthReport[]>;
  private _dataset: BehaviorSubject<HealthReport[]>;
  private dataStore: {
    dataset: HealthReport[];
  };
  httpGetSubscription: Subscription;

  constructor(private http: HttpClient, private authService: AuthService, private configDataService: ConfigDataService) {
    this.dataStore = { dataset: [] };
    this._dataset = new BehaviorSubject([]) as BehaviorSubject<HealthReport[]>;
    this.dataset = this._dataset.asObservable();
  }

  clear() {
    this.dataStore.dataset = [];
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }

  AddHealthCheck(baseUrl: string, servicename: string) {

    const init: HealthReport = { status: 'Loading', servicename, duration: '0', results: [] };
    this.addtothelist(init, baseUrl, servicename);

    this.httpGetSubscription = this.http.get<HealthReport>(baseUrl).pipe(map((response) => response)).subscribe((data) => {
      this.addtothelist(data, baseUrl, servicename);
      console.log('Loading completed');
    }, (error) => {
      if (error.status === 503) {
        console.log('Could not load items.' + JSON.stringify(error));
        this.addtothelist(error.error, baseUrl, servicename);
      } else {
        const data: HealthReport = {
          status: 'Unhealthy', servicename, duration: '0', results: [
            {
              type: 'IsAliveRequestFailed', description: '', duration: '0', status: 'Unhealthy',
              name: 'Request Failed', data: { url: baseUrl, status: error.status }, exception: null,
            },
          ],
        };
        this.addtothelist(data, baseUrl, servicename);
      }
    });
  }

  GetHealthCheckUrls(application: string, environment: string): Observable<HealthReportUrl[]> {
    const obs = Observable.create((observer) => {
      const data = this.configDataService.getHealthCheckUrls(application, environment);
      observer.next(data);
    });
    return obs;
  }

  private handleError(error: any, observer: any, errorMessage: string) {
    console.error('An error occurred', error);
    if (error && error['_body']) { // check response here.
      const message = error.json();
    }
    observer.error(error.message || error);
  }

  private addtothelist(data: HealthReport, baseUrl: string, servicename: string) {
    data.url = baseUrl;
    data.servicename = servicename;
    if (this.dataStore.dataset && this.dataStore.dataset.length !== undefined) {
      const item = this.dataStore.dataset.find((x) => x.url === baseUrl);
      if (item) {
        const itemIndex = this.dataStore.dataset.findIndex((x) => x.url === baseUrl);
        if (itemIndex !== undefined) {
          this.dataStore.dataset[itemIndex] = data;
        }
      } else {
        this.dataStore.dataset.push(data);
      }
      this._dataset.next(Object.assign({}, this.dataStore).dataset);
    }
  }

  ngOnDestroy(): void {
    if (this.httpGetSubscription) { this.httpGetSubscription.unsubscribe(); }
  }
}
