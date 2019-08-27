import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-event-hub-health-checks',
  templateUrl: './event-hub-health-checks.component.html',
  styleUrls: ['./event-hub-health-checks.component.scss'],
})
export class EventHubHealthChecksComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }
}
