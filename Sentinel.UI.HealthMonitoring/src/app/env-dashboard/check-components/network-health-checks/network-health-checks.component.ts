import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-network-health-checks',
  templateUrl: './network-health-checks.component.html',
  styleUrls: ['./network-health-checks.component.scss'],
})
export class NetworkHealthChecksComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }
}
