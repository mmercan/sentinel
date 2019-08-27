import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-service-client-base-health-check',
  templateUrl: './service-client-base-health-check.component.html',
  styleUrls: ['./service-client-base-health-check.component.scss']
})
export class ServiceClientBaseHealthCheckComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }

}
