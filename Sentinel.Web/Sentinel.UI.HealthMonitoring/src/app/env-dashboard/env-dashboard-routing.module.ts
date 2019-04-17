import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProgDashboardComponent } from './prog-dashboard/prog-dashboard.component';
const routes: Routes = [{
  path: '',
  component: ProgDashboardComponent,
  data: {
    heading: 'Program Dashboard'
  }
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EnvDashboardRoutingModule { }
