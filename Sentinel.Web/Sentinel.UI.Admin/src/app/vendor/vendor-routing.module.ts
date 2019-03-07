import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdalGuard } from '../shared/authentication/adal-auth/adal.guard';

import { ListComponent } from './list/list.component';
const routes: Routes = [
  {
    path: 'list',
    component: ListComponent,
    data: {
      heading: 'Vendor List'
    },
    canActivate: [AdalGuard]
  }, {
    path: '',
    redirectTo: 'list'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VendorRoutingModule { }
