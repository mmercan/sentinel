import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';

import { SharedModule } from '../shared/shared.module';
import { AddMaxValueCheckComponent } from './check-components/add-max-value-check/add-max-value-check.component';
import { AddMinValueCheckComponent } from './check-components/add-min-value-check/add-min-value-check.component';
import { DbConnectionHealthChecksComponent } from './check-components/db-connection-health-checks/db-connection-health-checks.component';
import { DiHealthCheckComponent } from './check-components/di-health-check/di-health-check.component';
import { EventHubHealthChecksComponent } from './check-components/event-hub-health-checks/event-hub-health-checks.component';
import {
  IsAliveRequestFailedCheckComponent,
} from './check-components/is-alive-request-failed-check/is-alive-request-failed-check.component';
import { MongoHealthCheckComponent } from './check-components/mongo-health-check/mongo-health-check.component';
import { NetworkHealthChecksComponent } from './check-components/network-health-checks/network-health-checks.component';
import { PerformanceCounterListComponent } from './check-components/performance-counter-list/performance-counter-list.component';
import { ProcessListHealthChecksComponent } from './check-components/process-list-health-checks/process-list-health-checks.component';
import { RabbitMqHealthCheckComponent } from './check-components/rabbit-mq-health-check/rabbit-mq-health-check.component';
import { ServiceBusHealthChecksComponent } from './check-components/service-bus-health-checks/service-bus-health-checks.component';
import {
  ServiceClientBaseHealthCheckComponent,
} from './check-components/service-client-base-health-check/service-client-base-health-check.component';
import { SqlHealthChecksComponent } from './check-components/sql-health-checks/sql-health-checks.component';
import { SystemInfoHealthChecksComponent } from './check-components/system-info-health-checks/system-info-health-checks.component';
import { EnvDashboardRoutingModule } from './env-dashboard-routing.module';
import { ImportConfigComponent } from './import-config/import-config.component';
import { ProgDashboardComponent } from './prog-dashboard/prog-dashboard.component';
import { RedisHealthCheckComponent } from './check-components/redis-health-check/redis-health-check.component';

@NgModule({
  imports: [
    CommonModule,
    EnvDashboardRoutingModule,
    SharedModule,
    NgbCollapseModule,
  ],
  declarations: [ProgDashboardComponent, DiHealthCheckComponent, ProcessListHealthChecksComponent, PerformanceCounterListComponent,
    SystemInfoHealthChecksComponent, AddMaxValueCheckComponent, AddMinValueCheckComponent, DbConnectionHealthChecksComponent,
    ServiceClientBaseHealthCheckComponent, RabbitMqHealthCheckComponent, MongoHealthCheckComponent, IsAliveRequestFailedCheckComponent,
    ImportConfigComponent, SqlHealthChecksComponent, ServiceBusHealthChecksComponent, EventHubHealthChecksComponent,
    NetworkHealthChecksComponent,
    RedisHealthCheckComponent],
})
export class EnvDashboardModule { }
