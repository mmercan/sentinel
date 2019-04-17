import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonDataStoreService } from '../common-data-store/common-data-store.service';
import { HealthReport, HealthReportEntry } from './interfaces/health-report';
import { AppConfig } from '../../../app.config';
import { CommonDataStoreInterface } from '../common-data-store/common-data-store-interface';
import { AuthService } from '../../authentication/auth.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class HealthcheckDataStoreService {
  public dataset: Observable<HealthReport>;
  private _dataset: BehaviorSubject<HealthReport>;

  private dataStore: {
    dataset: HealthReport
  };

  constructor(private http: HttpClient, private authService: AuthService, appConfig: AppConfig) {
    this.dataStore = { dataset: null };
    this._dataset = <BehaviorSubject<HealthReport>>new BehaviorSubject({});
    this.dataset = this._dataset.asObservable();
  }

  getAll(baseUrl: string) {
    // const obs = Observable.create(observer => {

    this.http.get<HealthReport>(baseUrl).pipe(map(response => response)).subscribe(data => {
      // this.dataStore.dataset = data;
      // setTimeout(_ => this._dataset.next(Object.assign({}, this.dataStore).dataset));
      this._dataset.next(Object.assign({}, this.dataStore).dataset);

      console.log('Loading completed');
      // observer.next(Object.assign({}, this.dataStore).dataset);
    }, error => {
      if (error.status === 503) {
        console.log('Could not load items.' + JSON.stringify(error));
        this.dataStore.dataset = error.error;
        this._dataset.next(Object.assign({}, this.dataStore).dataset);
      } else {

      }

    });
  }

}
