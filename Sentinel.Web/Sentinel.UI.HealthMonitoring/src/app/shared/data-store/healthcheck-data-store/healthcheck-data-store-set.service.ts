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
export class HealthcheckDataStoreSetService {

  public dataset: Observable<HealthReport[]>;
  private _dataset: BehaviorSubject<HealthReport[]>;
  private dataStore: {
    dataset: HealthReport[];
  };

  constructor(private http: HttpClient, private authService: AuthService, appConfig: AppConfig) {
    this.dataStore = { dataset: [] };
    this._dataset = <BehaviorSubject<HealthReport[]>>new BehaviorSubject([]);
    this.dataset = this._dataset.asObservable();
  }

  AddHealthCheck(baseUrl: string, servicename: string) {
    this.http.get<HealthReport>(baseUrl).pipe(map(response => response)).subscribe(data => {
      this.addtothelist(data, baseUrl, servicename);
      console.log('Loading completed');
    }, error => {
      if (error.status === 503) {
        console.log('Could not load items.' + JSON.stringify(error));
        this.addtothelist(error.error, baseUrl, servicename);
      } else {

      }

    });
  }


  private addtothelist(data: HealthReport, baseUrl: string, servicename: string) {
    data.url = baseUrl;
    data.servicename = servicename;
    if (this.dataStore.dataset && this.dataStore.dataset.length !== undefined) {
      let item = this.dataStore.dataset.find(x => x.url === baseUrl);
      if (item) {
        item = data;
      } else {
        this.dataStore.dataset.push(data);
      }
      this._dataset.next(Object.assign({}, this.dataStore).dataset);
    }
  }
}
