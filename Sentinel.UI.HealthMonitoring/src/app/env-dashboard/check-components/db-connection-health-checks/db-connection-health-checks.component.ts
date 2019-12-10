import { Component, Input, OnInit } from '@angular/core';
import { IHealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-db-connection-health-checks',
  templateUrl: './db-connection-health-checks.component.html',
  styleUrls: ['./db-connection-health-checks.component.scss'],
})
export class DbConnectionHealthChecksComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: IHealthReportEntry;
  ngOnInit() {
  }
}
