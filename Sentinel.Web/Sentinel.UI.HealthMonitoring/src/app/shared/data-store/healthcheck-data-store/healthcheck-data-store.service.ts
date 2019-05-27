import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonDataStoreService } from '../common-data-store/common-data-store.service';
import { HealthReport, HealthReportEntry } from './interfaces/health-report';
import { AppConfig } from '../../../app.config';
import { CommonDataStoreInterface } from '../common-data-store/common-data-store-interface';
import { AuthService } from '../../authentication/auth.service';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class HealthcheckDataStoreService implements OnDestroy {
  public dataset: Observable<HealthReport>;
  private _dataset: BehaviorSubject<HealthReport>;

  private dataStore: {
    dataset: HealthReport
  };
  httpGetSubscription: Subscription;

  constructor(private http: HttpClient, private authService: AuthService, appConfig: AppConfig) {
    this.dataStore = { dataset: null };
    this._dataset = <BehaviorSubject<HealthReport>>new BehaviorSubject({});
    this.dataset = this._dataset.asObservable();
  }

  getAll(baseUrl: string, servicename: string) {
    // const obs = Observable.create(observer => {

    this.httpGetSubscription = this.http.get<HealthReport>(baseUrl).pipe(map(response => response))
      .subscribe(data => {
        data.url = baseUrl;
        data.servicename = servicename;
        this.dataStore.dataset = data;
        // setTimeout(_ => this._dataset.next(Object.assign({}, this.dataStore).dataset));
        this._dataset.next(Object.assign({}, this.dataStore).dataset);

        console.log('Loading completed');
        // observer.next(Object.assign({}, this.dataStore).dataset);
      }, error => {
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
    this.httpGetSubscription.unsubscribe();
  }

}
