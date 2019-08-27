import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-performance-counter-list',
  templateUrl: './performance-counter-list.component.html',
  styleUrls: ['./performance-counter-list.component.scss'],
})
export class PerformanceCounterListComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }

}
