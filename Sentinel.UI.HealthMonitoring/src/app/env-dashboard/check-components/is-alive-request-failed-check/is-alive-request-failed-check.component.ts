import { Component, Input, OnInit } from '@angular/core';
import { IHealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-is-alive-request-failed-check',
  templateUrl: './is-alive-request-failed-check.component.html',
  styleUrls: ['./is-alive-request-failed-check.component.scss'],
})
export class IsAliveRequestFailedCheckComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: IHealthReportEntry;

  ngOnInit() {
  }

}
