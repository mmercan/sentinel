import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../../authentication/auth.service';
import { HttpClient } from '@angular/common/http';

// @Injectable()
export class CommonAuthDataStoreService<T>  {

  public dataset: Observable<T[]>;
  // public keyProperyName: string;
  // public baseUrl: string;
  private _dataset: BehaviorSubject<T[]>;

  private dataStore: {
    dataset: T[]
  };
  authGetSubscription: Subscription;
  authPostSubscription: Subscription;
  authPutSubscription: Subscription;
  authDeleteSubscription: Subscription;


  constructor(private authService: AuthService, protected baseUrl: string, protected keyProperyName: string) {
    // this.baseUrl = 'http://56e05c3213da80110013eba3.mockapi.io/api';
    this.dataStore = { dataset: [] };
    this._dataset = <BehaviorSubject<T[]>>new BehaviorSubject([]);
    this.dataset = this._dataset.asObservable();
  }

  getAll() {
    // const obs = Observable.create(observer => {

    this.authGetSubscription = this.authService.authGet(this.baseUrl).subscribe(data => {
      this.dataStore.dataset = data;
      // setTimeout(_ => this._dataset.next(Object.assign({}, this.dataStore).dataset));
      this._dataset.next(Object.assign({}, this.dataStore).dataset);

      console.log('Loading completed');
      // observer.next(Object.assign({}, this.dataStore).dataset);
    }, error => console.log('Could not load items.'));

    // });
    // return obs;
  }

  get(id: number | string) {
    this.authService.authGet(`${this.baseUrl}/${id}`).subscribe(data => {
      let notFound = true;

      this.dataStore.dataset.forEach((item, index) => {
        if (this.getKeyField(item) === this.getKeyField(data)) {
          this.dataStore.dataset[index] = data;
          notFound = false;
        }
      });

      if (notFound) {
        this.dataStore.dataset.push(data);
      }

      this._dataset.next(Object.assign({}, this.dataStore).dataset);
    }, error => console.log(`Could not load item with the id. ${id}`));
  }

  private getKeyField(item: T): any {
    if (this.keyProperyName && item && item[this.keyProperyName]) {
      return item[this.keyProperyName];
    } else if (item && item['id']) {
      return item['id'];
    } else if (item && item['Id']) {
      return item['Id'];
    } else {
      return undefined;
    }
  }

  create(item: T) {
    this.authPostSubscription = this.authService.authPost(this.baseUrl, JSON.stringify(item))
      .subscribe(data => {
        this.dataStore.dataset.push(data);
        this._dataset.next(Object.assign({}, this.dataStore).dataset);
      }, error => console.log('Could not create items.'));
  }


  update(item: T) {
    const id = this.getKeyField(item);
    this.authPutSubscription = this.authService.authPut(this.baseUrl, JSON.stringify(item))
      .subscribe(data => {
        this.dataStore.dataset.forEach((t, i) => {
          if (this.getKeyField(t) === this.getKeyField(data)) { this.dataStore.dataset[i] = data; }
        });

        this._dataset.next(Object.assign({}, this.dataStore).dataset);
      }, error => console.log('Could not update item.'));
  }

  remove(id: number | string) {
    this.authDeleteSubscription = this.authService.authDelete(`${this.baseUrl}/${id}`).subscribe(response => {
      this.dataStore.dataset.forEach((t, i) => {
        if (this.getKeyField(t) === id) { this.dataStore.dataset.splice(i, 1); }
      });

      this._dataset.next(Object.assign({}, this.dataStore).dataset);
    }, error => console.log('Could not delete item.'));
  }

  OnDestroy(): void {
    if (this.authGetSubscription) { this.authGetSubscription.unsubscribe(); }
    if (this.authPostSubscription) { this.authPostSubscription.unsubscribe(); }
    if (this.authPostSubscription) { this.authPostSubscription.unsubscribe(); }
    if (this.authDeleteSubscription) { this.authDeleteSubscription.unsubscribe(); }
  }
}
