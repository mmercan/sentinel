import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-service-bus-health-checks',
  templateUrl: './service-bus-health-checks.component.html',
  styleUrls: ['./service-bus-health-checks.component.scss'],
})
export class ServiceBusHealthChecksComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }
}
