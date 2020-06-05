import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

import { DashboardComponent } from './dashboard.component';
import { DashboardRoutes } from './dashboard.routing';
import { SharedModule } from '../shared/shared.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
@NgModule({
  imports: [CommonModule, SharedModule, NgxChartsModule, RouterModule.forChild(DashboardRoutes)],
  declarations: [DashboardComponent]
})

export class DashboardModule { }
