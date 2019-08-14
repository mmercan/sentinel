import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import * as moment from 'moment';

export interface Notification {
  id: number;
  title: string;
  description?: string;
  date: Date;
  urltogoback?: string;
  humanizeDate?: string;
  showtoastr?: boolean;
}

@Injectable()
export class NotificationService {

  maxid = 1;
  public dataset: Observable<Notification[]>;
  private _dataset: BehaviorSubject<Notification[]>;

  private dataStore: {
    dataset: Notification[]
  };

  constructor() {
    this.dataStore = { dataset: [] };

    this._dataset = <BehaviorSubject<Notification[]>>new BehaviorSubject([]);
    this.dataset = this._dataset.asObservable();


    const notif: Notification = {
      id: 1,
      title: 'welcome',
      description: 'Welcome to Dashroad 2019-03-08-01',
      date: new Date(),

    };

    this.dataStore.dataset.push(notif);
    this._dataset.next(Object.assign({}, this.dataStore).dataset);

  }

  refresh() {
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }

  snapshot(): Notification[] {
    const snapshop = [];
    this.dataStore.dataset.slice(0, 5).forEach((item, index) => {
      item.humanizeDate = moment(item.date).fromNow();
      snapshop.push(item);
    });
    return snapshop;
  }

  remove(notification: Notification) {
    this.dataStore.dataset.forEach((t, i) => {
      if (t === notification) { this.dataStore.dataset.splice(i, 1); }
    });
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }

  showSuccess(title: string, description: string, showtoastr?: boolean) {
    this.dataStore.dataset.push(this.newNotification(title, description, showtoastr));
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }

  showError(title: string, description: string, showtoastr?: boolean) {
    this.dataStore.dataset.push(this.newNotification(title, description, showtoastr));
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }

  showVerbose(title: string, description: string, showtoastr?: boolean) {
    this.dataStore.dataset.push(this.newNotification(title, description, showtoastr));
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }

  showWarning(title: string, description: string, showtoastr?: boolean) {
    this.dataStore.dataset.push(this.newNotification(title, description, showtoastr));
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }

  showInfo(title: string, description: string, showtoastr?: boolean) {
    this.dataStore.dataset.push(this.newNotification(title, description, showtoastr));
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }

  showCustom(title: string, description: string, showtoastr?: boolean) {
    this.dataStore.dataset.push(this.newNotification(title, description, showtoastr));
    this._dataset.next(Object.assign({}, this.dataStore).dataset);
  }


  newNotification(title: string, description: string, popup: boolean): Notification {
    const notif = {
      id: this.maxid,
      date: new Date(),
      title: title,
      description: description,
      popup: false
    };
    this.maxid++;
    return notif;
  }

}
