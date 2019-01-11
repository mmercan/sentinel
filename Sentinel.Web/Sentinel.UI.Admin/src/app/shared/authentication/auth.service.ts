import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { AppConfig, authenticationType, logLevel } from '../../app.config';
import { Subscription, Observable, Subject } from 'rxjs';
import { Notification, NotificationService } from '../notification/notification.service';

import { AdalService } from './adal-auth/adal.service';
import { LocalAuthService } from './local-auth/local-auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokeyKey = 'token';
  private internal: any;
  private token: string;
  private isLoggedinValue: boolean;
  private status = new Subject<boolean>();
  // public user: Observable<any>;



  constructor(
    private appConfig: AppConfig,
    private http: Http,
    private notificationService: NotificationService,
    private adalService: AdalService,
    private localAuthService: LocalAuthService
  ) {

  }

  private handleError(error: any, observer: any, errorMessage: string) {
    console.error('An error occurred', error);
    if (error && error['_body']) { // check response here.
      const message = error.json();
    }
    this.notificationService.showError(errorMessage, <string>error, true);
    observer.error(error.message || error);
  }

  checkLogin(): Observable<boolean> {
    if (this.appConfig.config.authenticationType === authenticationType.Adal) {
      const obs = Observable.create(observer => {
        observer.next(this.adalService.userInfo.authenticated);
      });
      return obs;
    } else {
      return this.localAuthService.isLoggedin();
    }
  }


  login(userName?: string, password?: string): Observable<any> {
    if (this.appConfig.config.authenticationType === authenticationType.Adal) {
      this.adalService.login();
      const obs = Observable.create(observer => {
      });
      return obs;
    } else {
      return this.localAuthService.login(userName, password);
    }
  }


  logout() {
    if (this.appConfig.config.authenticationType === authenticationType.Adal) {
      this.adalService.logOut();
    } else {
      this.localAuthService.logout();
    }
  }

  authenticated(): boolean {
    if (this.appConfig.config.authenticationType === authenticationType.Adal) {
      return this.adalService.userInfo.authenticated;
    } else {
      return this.localAuthService.checkLogin();
    }
  }

  getUserInfo(): Observable<any> {
    const obs = Observable.create(observer => {
      if (this.appConfig.config.authenticationType === authenticationType.Adal) {
        this.adalService.getUser().subscribe(
          data => { observer.next(data); },
          error => { observer.error(error); });
      } else {
        this.localAuthService.user.subscribe(
          data => { observer.next(data); },
          error => { observer.error(error); });
      }
    });
    return obs;
  }

  getLocalToken(): string {
    if (this.appConfig.config.authenticationType === authenticationType.Adal) {
      return this.adalService.getCachedToken(this.appConfig.config.adalConfig.clientId);
    } else {
      return this.localAuthService.getLocalToken();
    }
  }

  handleWindowCallback() {
    if (this.appConfig.config.authenticationType === authenticationType.Adal) {
      return this.adalService.handleWindowCallback();
    } else {
      console.log('authentication is local no need for handleWindowCallback');
    }
  }

  authGet(url): Observable<any> {
    const headers = this.initAuthHeaders();
    const obs = Observable.create(observer => {
      let result = null;
      this.http.get(url, { headers: headers })
        .subscribe(
          response => {
            if (response.json) {
              result = response.json();
            } else if (response.text) {
              result = response.text();
            }
            observer.next(result);
          },
          error => { this.handleError(error, observer, 'Failed Get on' + url); }
        );

    });
    return obs;
  }

  authPost(url: string, body: any): Observable<any> {
    const obs = Observable.create(observer => {
      const headers = this.initAuthHeaders();
      let result = null;
      return this.http.post(url, body, { headers: headers })
        .subscribe(
          response => {
            if (response.ok) {
              if (response.text() === '') {
                observer.next(result);
              } else if (response.json) {
                result = response.json();
              } else if (response.text) {
                result = response.text();
              }
            }

            observer.next(result);
          },
          error => { this.handleError(error, observer, 'Failed Post on ' + url); }
        );
    });
    return obs;
  }

  authPut(url: string, body: any): Observable<any> {
    const obs = Observable.create(observer => {
      const headers = this.initAuthHeaders();
      let result = null;
      return this.http.put(url, body, { headers: headers })
        .subscribe(
          response => {
            if (response.ok) {
              if (response.text() === '') {
                observer.next(result);
              } else if (response.json) {
                result = response.json();
              } else if (response.text) {
                result = response.text();
              }
            }

            observer.next(result);
          },
          error => { this.handleError(error, observer, 'Failed Put on ' + url); }
        );
    });
    return obs;
  }

  authDelete(url): Observable<any> {
    const headers = this.initAuthHeaders();
    const obs = Observable.create(observer => {
      let result = null;
      this.http.delete(url, { headers: headers })
        .subscribe(
          response => {
            if (response.json) {
              result = response.json();
            } else if (response.text) {
              result = response.text();
            }
            observer.next(result);
          },
          error => { this.handleError(error, observer, 'Failed Delete on ' + url); }
        );

    });
    return obs;
  }


  private initAuthHeaders(): Headers {
    const token = this.getLocalToken();
    if (token == null) { throw new Error('No token'); }

    const headers = new Headers();
    headers.append('Content-Type', 'application/json');
    headers.append('Authorization', 'Bearer ' + token);
    return headers;
  }
}
