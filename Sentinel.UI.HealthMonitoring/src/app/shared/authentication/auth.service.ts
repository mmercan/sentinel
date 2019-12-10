import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { Observable, Subject, Subscription } from 'rxjs';
import { AppConfig, authenticationType, logLevel } from '../../app.config';
import { Notification, NotificationService } from '../notification/notification.service';
import { AdalService } from './adal-auth/adal.service';
import { LocalAuthService } from './local-auth/local-auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService implements OnDestroy {
  private tokeyKey = 'token';
  private internal: any;
  private token: string;
  private isLoggedinValue: boolean;
  private status = new Subject<boolean>();
  getUserSubscription: Subscription;
  httpGetSubscription: Subscription;
  httpDeleteSubscription: Subscription;
  httpPutSubscription: Subscription;
  httpPostSubscription: Subscription;
  // public user: Observable<any>;

  constructor(
    private appConfig: AppConfig,
    private http: HttpClient,
    private notificationService: NotificationService,
    private adalService: AdalService,
    private localAuthService: LocalAuthService,
  ) {

  }

  private handleError(error: any, observer: any, errorMessage: string) {
    console.error('An error occurred', error);
    if (error && error['_body']) { // check response here.
      const message = error.json();
    }
    this.notificationService.showError(errorMessage, error as string, true);
    observer.error(error.message || error);
  }

  checkLogin(): Observable<boolean> {
    if (this.appConfig.config.authenticationType === authenticationType.Adal) {
      const obs = Observable.create((observer) => {
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
      const obs = Observable.create((observer) => {
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
    const obs = Observable.create((observer) => {
      if (this.appConfig.config.authenticationType === authenticationType.Adal) {
        this.getUserSubscription = this.adalService.getUser().subscribe(
          (data) => { observer.next(data); },
          (error) => { observer.error(error); });
      } else {
        this.getUserSubscription = this.localAuthService.user.subscribe(
          (data) => { observer.next(data); },
          (error) => { observer.error(error); });
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

  // JwtInterceptor used to attach jwt token to the request check app.module.ts
  authGet(url): Observable<any> {
    const obs = Observable.create((observer) => {
      this.httpGetSubscription = this.http.get(url, { observe: 'response' })
        .subscribe(
          (response) => {

            observer.next(response.body);
          },
          (error) => { this.handleError(error, observer, 'Failed Get on' + url); },
        );

    });
    return obs;
  }

  // JwtInterceptor used to attach jwt token to the request check app.module.ts
  authPost(url: string, body: any): Observable<any> {
    return this.authPostWithHeader(url, body, null);
  }

  // JwtInterceptor used to attach jwt token to the request check app.module.ts
  authPostWithHeader(url: string, body: any, extraheaders?: Array<{ key: string, value: string }>): Observable<any> {
    const obs = Observable.create((observer) => {

      this.httpPostSubscription = this.http.post(url, body, { observe: 'response' })
        .subscribe(
          (response) => {
            if (response.ok) {
              observer.next(response.body);
            }
            observer.next(response.body);
          },
          (error) => { this.handleError(error, observer, 'Failed Post on ' + url); },
        );
    });
    return obs;
  }

  // JwtInterceptor used to attach jwt token to the request check app.module.ts
  authPut(url: string, body: any): Observable<any> {
    const obs = Observable.create((observer) => {
      this.httpPutSubscription = this.http.put(url, body, { observe: 'response' })
        .subscribe(
          (response) => {
            observer.next(response.body);
          },
          (error) => { this.handleError(error, observer, 'Failed Put on ' + url); },
        );
    });
    return obs;
  }

  // JwtInterceptor used to attach jwt token to the request check app.module.ts
  authDelete(url): Observable<any> {
    const obs = Observable.create((observer) => {
      this.httpDeleteSubscription = this.http.delete(url, { observe: 'response' })
        .subscribe(
          (response) => {
            observer.next(response.body);
          },
          (error) => { this.handleError(error, observer, 'Failed Delete on ' + url); },
        );
    });
    return obs;
  }

  ngOnDestroy(): void {
    if (this.getUserSubscription) { this.getUserSubscription.unsubscribe(); }
    if (this.httpGetSubscription) { this.httpGetSubscription.unsubscribe(); }
    if (this.httpDeleteSubscription) { this.httpDeleteSubscription.unsubscribe(); }
    if (this.httpPutSubscription) { this.httpPutSubscription.unsubscribe(); }
    if (this.httpPostSubscription) { this.httpPostSubscription.unsubscribe(); }
  }
}
