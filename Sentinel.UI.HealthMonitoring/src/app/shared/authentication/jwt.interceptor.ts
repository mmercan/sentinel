import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { mergeMap, tap } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { NotificationService, Notification } from '../notification/notification.service';
@Injectable({
  providedIn: 'root'
})
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private notificationService: NotificationService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // if the endpoint is not registered then pass
    // the request as it is to the next handler
    const token = this.authService.getLocalToken();
    if (!token) {
      return next.handle(req.clone());
    }

    // if the user is not authenticated then drop the request
    if (!this.authService.authenticated) {
      throw new Error('Cannot send request to registered endpoint if the user is not authenticated.');
    }

    // if the endpoint is registered then acquire and inject token
    let headers = req.headers || new HttpHeaders();
    // return this.adal.acquireToken(token).pipe(
    //     mergeMap((token: string) => {
    // inject the header
    headers = headers.append('Authorization', 'Bearer ' + token);
    headers = headers.set('Content-Type', 'application/json');
    return next.handle(req.clone({ headers: headers })).pipe(tap((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        // do stuff with response if you want
      }
    }, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          this.notificationService.showError('Authentication Required', 'Please login first');
          // this.auth.collectFailedRequest(request);
        }
      }
    }));
    // }
    // )
    // );
  }
}







