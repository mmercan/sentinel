import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ImportConfigComponent } from './import-config/import-config.component';
import { ProgDashboardComponent } from './prog-dashboard/prog-dashboard.component';

const routes: Routes = [{
  path: 'import-config',
  component: ImportConfigComponent,
  data: {
    heading: 'import Config',
  },
}, {
  path: '',
  component: ProgDashboardComponent,
  data: {
    heading: 'Program Dashboard',
  },
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EnvDashboardRoutingModule { }
