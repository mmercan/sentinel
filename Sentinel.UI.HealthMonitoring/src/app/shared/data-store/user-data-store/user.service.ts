import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';

import { AppConfig } from '../../../app.config';
import { AuthService } from '../../authentication/auth.service';
@Injectable()
export class ProductService implements OnDestroy {

  idField = 'productId';
  apilocation = 'product/';

  itemSource: Observable<any[]>;
  private _itemSource: BehaviorSubject<any[]>;
  private baseUrl: string;
  private dataStore: {
    items: any[]
  };
  loadAllSubscription: Subscription;
  deleteSubscription: Subscription;
  updateSubscription: Subscription;
  createSubscription: Subscription;
  loadSubscription: Subscription;

  constructor(private http: HttpClient, private authService: AuthService, private appConfig: AppConfig) {
    this.baseUrl = appConfig.config.Api.baseUrl + this.apilocation;
    this.dataStore = { items: [] };
    this._itemSource = <BehaviorSubject<any[]>>new BehaviorSubject([]);
    this.itemSource = this._itemSource.asObservable();
  }

  loadAll() {
    this.loadAllSubscription = this.http.get<any[]>(this.baseUrl).subscribe(data => {
      this.dataStore.items = data;
      this._itemSource.next(Object.assign({}, this.dataStore).items);
    }, error => console.log('Could not load items.'));
  }

  load(id: number | string) {
    this.loadSubscription = this.http.get(`${this.baseUrl}/${id}`).subscribe(data => {
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
    this.createSubscription = this.http.post(this.baseUrl, JSON.stringify(item))
      .subscribe(data => {
        this.dataStore.items.push(data);
        this._itemSource.next(Object.assign({}, this.dataStore).items);
      }, error => console.log('Could not create item.'));
  }

  update(item: any) {
    this.updateSubscription = this.http.put(`${this.baseUrl}/${item[this.idField]}`, JSON.stringify(item))
      .subscribe(data => {
        this.dataStore.items.forEach((t, i) => {
          if (t[this.idField] === data[this.idField]) { this.dataStore.items[i] = data; }
        });

        this._itemSource.next(Object.assign({}, this.dataStore).items);
      }, error => console.log('Could not update todo.'));
  }

  remove(itemId: number) {
    this.deleteSubscription = this.http.delete(`${this.baseUrl}/${itemId}`).subscribe(response => {
      this.dataStore.items.forEach((t, i) => {
        if (t[this.idField] === itemId) { this.dataStore.items.splice(i, 1); }
      });

      this._itemSource.next(Object.assign({}, this.dataStore).items);
    }, error => console.log('Could not delete todo.'));
  }

  ngOnDestroy(): void {
    if (this.loadAllSubscription) { this.loadAllSubscription.unsubscribe(); }
    if (this.deleteSubscription) { this.deleteSubscription.unsubscribe(); }
    if (this.updateSubscription) { this.updateSubscription.unsubscribe(); }
    if (this.createSubscription) { this.createSubscription.unsubscribe(); }
    if (this.loadSubscription) { this.loadSubscription.unsubscribe(); }
  }
}
