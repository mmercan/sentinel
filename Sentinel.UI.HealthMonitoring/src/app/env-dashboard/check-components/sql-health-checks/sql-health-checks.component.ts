import { Component, Input, OnInit } from '@angular/core';
import { IHealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-sql-health-checks',
  templateUrl: './sql-health-checks.component.html',
  styleUrls: ['./sql-health-checks.component.scss'],
})
export class SqlHealthChecksComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: IHealthReportEntry;
  ngOnInit() {
  }
}
