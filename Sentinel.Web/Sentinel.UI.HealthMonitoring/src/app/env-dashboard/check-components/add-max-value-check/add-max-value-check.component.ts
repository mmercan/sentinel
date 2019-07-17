import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntry } from '../../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-add-max-value-check',
  templateUrl: './add-max-value-check.component.html',
  styleUrls: ['./add-max-value-check.component.scss'],
})
export class AddMaxValueCheckComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }

}
