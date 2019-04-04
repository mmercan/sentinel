import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, CanLoad, ActivatedRouteSnapshot, RouterStateSnapshot, Route } from '@angular/router';
import { Observable } from 'rxjs';
import { AdalService } from './adal.service';

@Injectable()
export class AdalGuard implements CanActivate, CanActivateChild, CanLoad {

    constructor(private adalService: AdalService) { }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        return this.adalService.userInfo.authenticated;
    }

    canLoad(route: Route): Observable<boolean> | Promise<boolean> | boolean {
        return this.adalService.userInfo.authenticated;
    }

    canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        return this.canActivate(childRoute, state);
    }
}
