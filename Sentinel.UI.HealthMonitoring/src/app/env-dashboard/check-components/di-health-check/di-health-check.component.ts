import { Component, Input, OnInit } from '@angular/core';
import { IHealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-di-health-check',
  templateUrl: './di-health-check.component.html',
  styleUrls: ['./di-health-check.component.scss'],
})
export class DiHealthCheckComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: IHealthReportEntry;
  ngOnInit() {
  }

}
