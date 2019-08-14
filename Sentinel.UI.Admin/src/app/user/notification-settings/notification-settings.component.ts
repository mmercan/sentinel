import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

import { AppConfig } from '../../app.config';
import { AuthService } from '../../shared/authentication/auth.service';
declare var Notification: any;

@Component({
  selector: 'app-notification-settings',
  templateUrl: './notification-settings.component.html',
  styleUrls: ['./notification-settings.component.scss']
})
export class NotificationSettingsComponent implements OnInit {
  sw: ServiceWorkerRegistration;
  email = 'mmercan@outlook.com';
  claims = '';
  pushNotification = {
    disabled: true,
    browserSupports: false,
    subscribed: false
  };

  pushNotificationDisabled = false;

  constructor(private appConfig: AppConfig, private authService: AuthService) {
    if ('serviceWorker' in navigator && 'PushManager' in window) {
      this.pushNotification.browserSupports = true;
      if (Notification.permission !== 'denied') {
        this.pushNotification.disabled = false;
      }
      navigator.serviceWorker.ready.then(
        sw => {
          this.sw = sw;
          sw.pushManager.getSubscription().then(
            s => {
              this.pushNotification.subscribed = s !== null;
            }
          );
        }
      );
    }
  }
  ngOnInit() {
  }
  pushChanged(event: any) {

    this.sw.pushManager.getSubscription().then(subs => {
      if (subs != null) {
        subs.unsubscribe();
        this.pushNotification.subscribed = false;
      } else {
        this.sw.pushManager.subscribe({
          userVisibleOnly: true,
          applicationServerKey: this.urlB64ToUint8Array(this.appConfig.config.Nofitication.publicKey)
        })
          // .then(newSub => fetch(this.appConfig.config.Nofitication.subscriptionRepoUrl, {
          //   headers: { 'Content-Type': 'application/json', 'email': this.email },
          //   method: 'POST',
          //   credentials: 'include',
          //   body: JSON.stringify(newSub),
          // }).then(repoResponse => {
          //   this.pushNotification.subscribed = true;
          // }));
          .then(newsub => this.authService.authPost(this.appConfig.config.Nofitication.subscriptionRepoUrl, JSON.stringify(newsub))
            .subscribe(repoResponse => {
              this.pushNotification.subscribed = true;
            }));
      }
    });

    // if (event.checked) {
    //   console.log('Checked');
    //   this.pushNotification.disabled = true;
    //   Notification.requestPermission()
    //     .then()
    //     .catch(error => {
    //       console.log(error);
    //     });
    //   this.pushNotification.disabled = false;
    // } else {
    //   console.log('UnChecked');
    //   this.pushNotification.disabled = true;


    //   this.pushNotification.disabled = false;
    // }
  }
  private urlB64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
      .replace(/\-/g, '+')
      .replace(/_/g, '/');

    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
      outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
  }
  callClaimns() {
    this.authService.authGet('http://localhost:5000/api/token/claimsjwt')
      .subscribe(Response => {
        this.claims = Response;
      });
  }


}
