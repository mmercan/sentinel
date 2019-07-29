import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

import { EnvDashboardRoutingModule } from './env-dashboard-routing.module';
import { ProgDashboardComponent } from './prog-dashboard/prog-dashboard.component';
import { SharedModule } from '../shared/shared.module';
import { DiHealthCheckComponent } from './check-components/di-health-check/di-health-check.component';
import { ProcessListHealthChecksComponent } from './check-components/process-list-health-checks/process-list-health-checks.component';
import { PerformanceCounterListComponent } from './check-components/performance-counter-list/performance-counter-list.component';
import { SystemInfoHealthChecksComponent } from './check-components/system-info-health-checks/system-info-health-checks.component';
import { AddMaxValueCheckComponent } from './check-components/add-max-value-check/add-max-value-check.component';
import { AddMinValueCheckComponent } from './check-components/add-min-value-check/add-min-value-check.component';
import { DbConnectionHealthChecksComponent } from './check-components/db-connection-health-checks/db-connection-health-checks.component';
// tslint:disable-next-line:max-line-length
import { ServiceClientBaseHealthCheckComponent } from './check-components/service-client-base-health-check/service-client-base-health-check.component';
import { RabbitMqHealthCheckComponent } from './check-components/rabbit-mq-health-check/rabbit-mq-health-check.component';
import { MongoHealthCheckComponent } from './check-components/mongo-health-check/mongo-health-check.component';
import { IsAliveRequestFailedCheckComponent } from './check-components/is-alive-request-failed-check/is-alive-request-failed-check.component';
import { ImportConfigComponent } from './import-config/import-config.component';


@NgModule({
  imports: [
    CommonModule,
    EnvDashboardRoutingModule,
    SharedModule,
    NgbCollapseModule
  ],
  declarations: [ProgDashboardComponent, DiHealthCheckComponent, ProcessListHealthChecksComponent, PerformanceCounterListComponent,
    SystemInfoHealthChecksComponent, AddMaxValueCheckComponent, AddMinValueCheckComponent, DbConnectionHealthChecksComponent,
    ServiceClientBaseHealthCheckComponent, RabbitMqHealthCheckComponent, MongoHealthCheckComponent, IsAliveRequestFailedCheckComponent, ImportConfigComponent]
})
export class EnvDashboardModule { }
