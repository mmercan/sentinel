import { Routes } from '@angular/router';
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthService } from '../shared/authentication/auth.service';

@Injectable()
export class OAuthCallbackHandler implements CanActivate {
    constructor(private router: Router, private authService: AuthService) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        this.authService.handleWindowCallback();
        if (this.authService.authenticated) {

            const returnUrl = route.queryParams['returnUrl'];
            if (!returnUrl) {
                this.router.navigate(['/']);
            } else {
                this.router.navigate([returnUrl], { queryParams: route.queryParams });
            }
        } else {
            this.router.navigate(['/']);
            // this.router.navigate(['signin']);
        }
        // if (this.adalService.userInfo) {
        //     const returnUrl = route.queryParams['returnUrl'];
        //     if (!returnUrl) {
        //         this.router.navigate(['/']);
        //     } else {
        //         this.router.navigate([returnUrl], { queryParams: route.queryParams });
        //     }
        // } else {
        //     this.router.navigate(['signin']);
        // }

        return true;
    }
}
