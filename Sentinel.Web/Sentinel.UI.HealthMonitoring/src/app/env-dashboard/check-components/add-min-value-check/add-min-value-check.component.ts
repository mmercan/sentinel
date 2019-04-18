import { Component, OnInit, Input } from '@angular/core';
import { HealthReportEntry } from 'src/app/shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-add-min-value-check',
  templateUrl: './add-min-value-check.component.html',
  styleUrls: ['./add-min-value-check.component.scss']
})
export class AddMinValueCheckComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }

}
