import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { AppConfig } from '../../../app.config';
import { AuthService } from '../../authentication/auth.service';
import { CommonDataStoreInterface } from '../common-data-store/common-data-store-interface';
import { CommonDataStoreService } from '../common-data-store/common-data-store.service';
import { IHealthReport, IHealthReportEntry } from './interfaces/health-report';

@Injectable({
  providedIn: 'root',
})
export class HealthcheckDataStoreService implements OnDestroy {
  public dataset: Observable<IHealthReport>;
  private _dataset: BehaviorSubject<IHealthReport>;

  private dataStore: {
    dataset: IHealthReport,
  };
  httpGetSubscription: Subscription;

  constructor(private http: HttpClient, private authService: AuthService, appConfig: AppConfig) {
    this.dataStore = { dataset: null };
    this._dataset = <BehaviorSubject<IHealthReport>>new BehaviorSubject({});
    this.dataset = this._dataset.asObservable();
  }

  getAll(baseUrl: string, servicename: string) {
    this.httpGetSubscription = this.http.get<IHealthReport>(baseUrl).pipe(map((response) => response))
      .subscribe((data) => {
        data.url = baseUrl;
        data.servicename = servicename;
        this.dataStore.dataset = data;
        // setTimeout(_ => this._dataset.next(Object.assign({}, this.dataStore).dataset));
        this._dataset.next(Object.assign({}, this.dataStore).dataset);

        console.log('Loading completed');
        // observer.next(Object.assign({}, this.dataStore).dataset);
      }, (error) => {
        if (error.status === 503) {
          console.log('Could not load items.' + JSON.stringify(error));
          error.error.url = baseUrl;
          error.error.servicename = servicename;
          this.dataStore.dataset = error.error;
          this._dataset.next(Object.assign({}, this.dataStore).dataset);
        } else {

        }
      });
  }

  ngOnDestroy(): void {
    if (this.httpGetSubscription) { this.httpGetSubscription.unsubscribe(); }
  }

}
