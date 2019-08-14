import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminLayoutComponent } from './layouts/admin/admin-layout.component';
import { AuthLayoutComponent } from './layouts/auth/auth-layout.component';
// import { AuthGuardService } from './shared/authentication/auth-guard.service';
import { AdalGuard } from './shared/authentication/adal-auth/adal.guard';
import { SettingsComponent } from './shared/settings/settings.component';

export const routes: Routes = [{
  path: '',
  component: AdminLayoutComponent,
  children: [{
    path: '',
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule)
  }, {
    path: 'id_token',
    loadChildren: () => import('./authentication/authentication.module').then(m => m.AuthenticationModule)
  },
  {
    path: 'user',
    loadChildren: () => import('./user/user.module').then(m => m.UserModule),
    canLoad: [AdalGuard]
  }, {
    path: 'docs',
    loadChildren: () => import('./docs/docs.module').then(m => m.DocsModule)
  }, {
    path: 'product',
    loadChildren: () => import('./product/product.module').then(m => m.ProductModule)
  }, {
    path: 'vendor',
    loadChildren: () => import('./vendor/vendor.module').then(m => m.VendorModule)
  }, {
    path: 'category',
    loadChildren: () => import('./category/category.module').then(m => m.CategoryModule)
  }, {
    path: 'settings',
    component: SettingsComponent,
    //   loadChildren: './settings/settings.module#SettingsModule'
  }
  ]
}, {
  path: '',
  component: AuthLayoutComponent,
  children: [{
    path: 'authentication',
    loadChildren: () => import('./authentication/authentication.module').then(m => m.AuthenticationModule)
  }, {
    path: 'error',
    loadChildren: () => import('./error/error.module').then(m => m.ErrorModule)
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
