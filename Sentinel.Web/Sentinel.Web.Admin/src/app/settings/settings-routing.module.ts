import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProductComponent } from './product/product.component';
import { AdalGuard } from '../shared/authentication/adal-auth/adal.guard';
const routes: Routes = [{
  path: 'product',
  component: ProductComponent,
  data: {
    heading: 'Product Settings'
  },
  canActivate: [AdalGuard]
}, {
  path: '',
  redirectTo: 'product'
}];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
