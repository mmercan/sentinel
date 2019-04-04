import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

@Injectable()
export class OfflineNotificationService {
  private _isOffline = false;
  private subject = new Subject<any>();
  constructor() {

    window.addEventListener('online', () => this.updateNetworkStatus(), false);
    window.addEventListener('offline', () => this.updateNetworkStatus(), false);

    if (navigator) {
      this._isOffline = !navigator.onLine;
    }


  }
  public isOffline(): boolean {
    return this._isOffline;
  }

  public GetStatus(): Observable<string> {
    return this.subject.asObservable();
  }

  private updateNetworkStatus() {
    if (navigator) {
      if (navigator.onLine) {
        this._isOffline = false;
        this.subject.next('online');
      } else {
        this._isOffline = true;
        this.subject.next('offline');
      }
    }
  }

}
