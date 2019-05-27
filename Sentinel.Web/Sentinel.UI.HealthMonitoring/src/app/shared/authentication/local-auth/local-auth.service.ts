import { Injectable, OnDestroy } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { AppConfig } from '../../../app.config';
import { Subscription, Observable, Subject } from 'rxjs';
import { Notification, NotificationService } from '../../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class LocalAuthService implements OnDestroy {
  private tokeyKey = 'token';
  private internal: any;
  private token: string;
  private isLoggedinValue: boolean;
  private status = new Subject<boolean>();
  public user: Observable<any>;
  loginSubscription: Subscription;

  constructor(
    private appConfig: AppConfig,
    private http: Http,
    private notificationService: NotificationService
  ) {
    if (this.checkLogin()) {
      this.isLoggedinValue = true;
      this.status.next(this.isLoggedinValue);
    } else {
      this.isLoggedinValue = false;
      this.status.next(this.isLoggedinValue);
    }
    // setTimeout(() => { this.checkLogin(); }, 1000);

    this.internal = setInterval(() => {
      this.checkLogin();
    }, 60000);
  }

  private handleError(error: any, observer: any, errorMessage: string) {
    console.error('An error occurred', error);
    if (error && error['_body']) { // check response here.
      const message = error.json();
    }
    this.notificationService.showError(errorMessage, <string>error, true);
    observer.error(error.message || error);
  }
  isLoggedin(): Observable<boolean> {
    return this.status.asObservable();
  }




  login(userName: string, password: string): Observable<any> {
    const obs = Observable.create(observer => {
      if (!this.appConfig.config.login.loginUrl) {
        this.handleError(null, observer, 'Login url is empty');
      }

      let opt: RequestOptions;
      const myHeaders: Headers = new Headers;
      myHeaders.set('Content-type', 'application/json');
      opt = new RequestOptions({
        headers: myHeaders
      });

      this.loginSubscription = this.http.post(this.appConfig.config.login.loginUrl, { Username: userName, Password: password }, opt)
        .subscribe(response => {
          const result = response.json();
          const json = result as any;
          sessionStorage.setItem('token', json.token);

          const oldDateObj = new Date();
          const expiresMinutes = json.expiresMinutes;
          const newDateObj = new Date(oldDateObj.getTime() + expiresMinutes * 60000);
          sessionStorage.setItem('expires', newDateObj.toString());

          this.checkLogin();
          observer.next(result);
        },
          error => { this.handleError(error, observer, 'Login Failed'); }
        );
    });
    return obs;
  }

  logout() {
    console.log('logout Triggered');
    sessionStorage.removeItem('expires');
    sessionStorage.removeItem('token');
    this.checkLogin();
    this.token = null;
    // sessionStorage.setItem('expires', newDateObj.toString());
  }

  checkLogin(): boolean {
    const token = sessionStorage.getItem('token');
    const expires = sessionStorage.getItem('expires');
    if (token && expires) {
      const now = new Date();
      const expireDate = new Date(expires);
      if (token && expireDate < now) {
        if (this.isLoggedinValue !== false) {
          this.status.next(false);
        }
        this.isLoggedinValue = false;
        return false;
      } else {
        if (this.isLoggedinValue !== true) {
          this.status.next(true);
        }
        this.isLoggedinValue = true;
        return true;
      }
    }
    const currentstatus = token != null;
    if (this.isLoggedinValue !== currentstatus) {
      this.status.next(currentstatus);
    }
    this.isLoggedinValue = currentstatus;
    return currentstatus;
  }

  getUserInfo(): Observable<any> {
    // return this.authGet('/api/TokenAuth');
    const obs = Observable.create(observer => {
    });
    return obs;
  }



  getLocalToken(): string {
    this.token = sessionStorage.getItem(this.tokeyKey);
    return this.token;
  }

  ngOnDestroy(): void {
    this.loginSubscription.unsubscribe();
  }
}
