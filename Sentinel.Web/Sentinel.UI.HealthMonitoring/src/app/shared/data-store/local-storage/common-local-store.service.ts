import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

// @Injectable()
export class CommonLocalStoreService<T> {
  public dataset: Observable<T>;
  private _dataset: BehaviorSubject<T>;
  private dataStore: {
    dataset: T
  };

  constructor(protected key: string) {

    const valstr = localStorage.getItem(key);
    const val = JSON.parse(valstr);

    this.dataStore = { dataset: val };
    this._dataset = <BehaviorSubject<T>>new BehaviorSubject(val);
    this.dataset = this._dataset.asObservable();

  }

  commit() {
    const valstr = JSON.stringify(this.dataStore.dataset);
    localStorage.setItem(this.key, valstr);
  }






}
