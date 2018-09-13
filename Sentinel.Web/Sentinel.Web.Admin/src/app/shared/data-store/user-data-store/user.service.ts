import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable, BehaviorSubject, Subject } from 'rxjs';
import { map } from 'rxjs/operators';

import { AppConfig } from '../../../app.config';
import { AuthService } from '../../authentication/auth.service';
@Injectable()
export class ProductService {
  idField = 'productId';
  apilocation = 'product/';

  itemSource: Observable<any[]>;
  private _itemSource: BehaviorSubject<any[]>;
  private baseUrl: string;
  private dataStore: {
    items: any[]
  };

  constructor(private http: Http, private authService: AuthService, private appConfig: AppConfig) {
    this.baseUrl = appConfig.config.Api.baseUrl + this.apilocation;
    this.dataStore = { items: [] };
    this._itemSource = <BehaviorSubject<any[]>>new BehaviorSubject([]);
    this.itemSource = this._itemSource.asObservable();
  }

  loadAll() {
    this.http.get(this.baseUrl).pipe(map(response => response.json())).subscribe(data => {
      this.dataStore.items = data;
      this._itemSource.next(Object.assign({}, this.dataStore).items);
    }, error => console.log('Could not load items.'));
  }

  load(id: number | string) {
    this.http.get(`${this.baseUrl}/${id}`).pipe(map(response => response.json())).subscribe(data => {
      let notFound = true;

      this.dataStore.items.forEach((item, index) => {
        if (item[this.idField] === data[this.idField]) {
          this.dataStore.items[index] = data;
          notFound = false;
        }
      });

      if (notFound) {
        this.dataStore.items.push(data);
      }

      this._itemSource.next(Object.assign({}, this.dataStore).items);
    }, error => console.log('Could not load items.'));
  }

  create(item: any) {
    this.http.post(this.baseUrl, JSON.stringify(item))
      .pipe(map(response => response.json())).subscribe(data => {
        this.dataStore.items.push(data);
        this._itemSource.next(Object.assign({}, this.dataStore).items);
      }, error => console.log('Could not create item.'));
  }

  update(item: any) {
    this.http.put(`${this.baseUrl}/${item[this.idField]}`, JSON.stringify(item))
      .pipe(map(response => response.json())).subscribe(data => {
        this.dataStore.items.forEach((t, i) => {
          if (t[this.idField] === data[this.idField]) { this.dataStore.items[i] = data; }
        });

        this._itemSource.next(Object.assign({}, this.dataStore).items);
      }, error => console.log('Could not update todo.'));
  }

  remove(itemId: number) {
    this.http.delete(`${this.baseUrl}/${itemId}`).subscribe(response => {
      this.dataStore.items.forEach((t, i) => {
        if (t[this.idField] === itemId) { this.dataStore.items.splice(i, 1); }
      });

      this._itemSource.next(Object.assign({}, this.dataStore).items);
    }, error => console.log('Could not delete todo.'));
  }
}
