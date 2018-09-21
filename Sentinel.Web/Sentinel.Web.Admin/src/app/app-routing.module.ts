import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminLayoutComponent } from './layouts/admin/admin-layout.component';
import { AuthLayoutComponent } from './layouts/auth/auth-layout.component';
// import { AuthGuardService } from './shared/authentication/auth-guard.service';
import { AdalGuard } from './shared/authentication/adal-auth/adal.guard';

export const routes: Routes = [{
  path: '',
  component: AdminLayoutComponent,
  children: [{
    path: '',
    loadChildren: './dashboard/dashboard.module#DashboardModule'
  }, {
    path: 'id_token',
    loadChildren: './authentication/authentication.module#AuthenticationModule'
  },
  {
    path: 'user',
    loadChildren: './user/user.module#UserModule',
    canLoad: [AdalGuard]
  }, {
    path: 'docs',
    loadChildren: './docs/docs.module#DocsModule'
  }, {
    path: 'product',
    loadChildren: './product/product.module#ProductModule'
  }]
}, {
  path: '',
  component: AuthLayoutComponent,
  children: [{
    path: 'authentication',
    loadChildren: './authentication/authentication.module#AuthenticationModule'
  }, {
    path: 'error',
    loadChildren: './error/error.module#ErrorModule'
  }]
}, {
  path: '**',
  redirectTo: 'error/404'
}];


@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
