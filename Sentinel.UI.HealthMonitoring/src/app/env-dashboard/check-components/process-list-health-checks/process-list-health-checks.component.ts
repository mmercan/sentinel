import { Component, Input, OnInit } from '@angular/core';
import { IHealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-process-list-health-checks',
  templateUrl: './process-list-health-checks.component.html',
  styleUrls: ['./process-list-health-checks.component.scss'],
})
export class ProcessListHealthChecksComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: IHealthReportEntry;
  ngOnInit() {
  }

}
