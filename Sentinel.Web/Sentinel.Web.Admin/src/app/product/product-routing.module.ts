import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from './list/list.component';
import { AdalGuard } from '../shared/authentication/adal-auth/adal.guard';
const routes: Routes = [{
  path: 'list',
  component: ListComponent,
  data: {
    heading: 'Product List'
  },
  canActivate: [AdalGuard]
}, {
  path: '',
  redirectTo: 'list'
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductRoutingModule { }
