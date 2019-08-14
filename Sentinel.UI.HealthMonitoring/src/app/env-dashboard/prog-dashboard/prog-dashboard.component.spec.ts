import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EnvDashboardModule } from '../env-dashboard.module';

import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared/shared.module';
import { AddMaxValueCheckComponent } from '../check-components/add-max-value-check/add-max-value-check.component';
import { AddMinValueCheckComponent } from '../check-components/add-min-value-check/add-min-value-check.component';
import { DbConnectionHealthChecksComponent } from '../check-components/db-connection-health-checks/db-connection-health-checks.component';
import { DiHealthCheckComponent } from '../check-components/di-health-check/di-health-check.component';
// tslint:disable-next-line:max-line-length
import { IsAliveRequestFailedCheckComponent } from '../check-components/is-alive-request-failed-check/is-alive-request-failed-check.component';
import { MongoHealthCheckComponent } from '../check-components/mongo-health-check/mongo-health-check.component';
import { PerformanceCounterListComponent } from '../check-components/performance-counter-list/performance-counter-list.component';
import { ProcessListHealthChecksComponent } from '../check-components/process-list-health-checks/process-list-health-checks.component';
import { RabbitMqHealthCheckComponent } from '../check-components/rabbit-mq-health-check/rabbit-mq-health-check.component';
// tslint:disable-next-line:max-line-length
import { ServiceClientBaseHealthCheckComponent } from '../check-components/service-client-base-health-check/service-client-base-health-check.component';
import { SystemInfoHealthChecksComponent } from '../check-components/system-info-health-checks/system-info-health-checks.component';
import { EnvDashboardRoutingModule } from '../env-dashboard-routing.module';
import { ImportConfigComponent } from '../import-config/import-config.component';
import { ProgDashboardComponent } from './prog-dashboard.component';

describe('ProgDashboardComponent', () => {
  let component: ProgDashboardComponent;
  let fixture: ComponentFixture<ProgDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, RouterModule.forRoot([]), SharedModule.forRoot(), NgbCollapseModule.forRoot()],
      declarations: [ProgDashboardComponent, DiHealthCheckComponent, ProcessListHealthChecksComponent, PerformanceCounterListComponent,
        SystemInfoHealthChecksComponent, AddMaxValueCheckComponent, AddMinValueCheckComponent, DbConnectionHealthChecksComponent,
        ServiceClientBaseHealthCheckComponent, RabbitMqHealthCheckComponent, MongoHealthCheckComponent,
        IsAliveRequestFailedCheckComponent,
      ],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    // tslint:disable-next-line:max-line-length
    const content = '{"provider": {"dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] }, "test": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "euat": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] }, "perf": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}] } }, "apollo": { "dev": {"healthChecks": [{"isaliveandwell": "https://healthmonitoring.api.myrcan.com/health/isaliveandwell","name": "HealthMonitoring Api"}, {"isaliveandwell": "https://comms.api.myrcan.com/health/isaliveandwell","name": "Comms Api"}, {"isaliveandwell": "https://member.api.myrcan.com/health/isaliveandwell","name": "Member Api"}, {"isaliveandwell": "https://product.api.myrcan.com/health/isaliveandwell","name": "Product Api"}, {"isaliveandwell": "https://product.myrcan.com/health/isaliveandwell","name": "Product UI"}, {"isaliveandwell": "https://auth.myrcan.com/health/isaliveandwell","name": "STS UI"}] } } }';
    localStorage.setItem('app-configData', content);

    fixture = TestBed.createComponent(ProgDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
