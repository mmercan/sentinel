import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-system-info-health-checks',
  templateUrl: './system-info-health-checks.component.html',
  styleUrls: ['./system-info-health-checks.component.scss']
})
export class SystemInfoHealthChecksComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }

}
