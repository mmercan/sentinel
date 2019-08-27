import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-mongo-health-check',
  templateUrl: './mongo-health-check.component.html',
  styleUrls: ['./mongo-health-check.component.scss']
})
export class MongoHealthCheckComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }

}
