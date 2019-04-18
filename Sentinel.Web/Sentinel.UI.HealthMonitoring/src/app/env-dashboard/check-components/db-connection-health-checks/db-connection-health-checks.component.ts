import { Component, OnInit, Input } from '@angular/core';
import { HealthReportEntry } from 'src/app/shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-db-connection-health-checks',
  templateUrl: './db-connection-health-checks.component.html',
  styleUrls: ['./db-connection-health-checks.component.scss']
})
export class DbConnectionHealthChecksComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }

}
