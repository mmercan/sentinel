import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../shared/authentication/auth.service';
import { Router, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AppConfig } from '../../app.config';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-oath-callback',
  templateUrl: './oath-callback.component.html',
  styleUrls: ['./oath-callback.component.scss']
})
export class OathCallbackComponent implements OnInit, OnDestroy {
  getUserInfoSubscription: Subscription;


  constructor(private router: Router, private authService: AuthService, private appConfig: AppConfig) {

  }

  ngOnInit() {
    if (this.authService.authenticated) {
      this.getUserInfoSubscription = this.authService.getUserInfo().subscribe(data => {
        console.log(data);
      });

      const oldDateObj = new Date();
      const expiresMinutes = 1500; // json.expiresMinutes;
      const newDateObj = new Date(oldDateObj.getTime() + expiresMinutes * 60000);
      localStorage.setItem('expires', newDateObj.toString());
      // sessionStorage.setItem('token', this.adalService.getCachedToken());

      this.router.navigate(['/']);
    } else {
      this.router.navigate(['login']);
    }
  }


  ngOnDestroy(): void {
    this.getUserInfoSubscription.unsubscribe();
  }
}


