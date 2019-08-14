import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutes } from './dashboard.routing';

@NgModule({
  imports: [CommonModule, RouterModule.forChild(DashboardRoutes)],
  declarations: [DashboardComponent],
})

export class DashboardModule { }
