import { Component, OnInit, Input } from '@angular/core';
import { HealthReportEntry } from 'src/app/shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-rabbit-mq-health-check',
  templateUrl: './rabbit-mq-health-check.component.html',
  styleUrls: ['./rabbit-mq-health-check.component.scss']
})
export class RabbitMqHealthCheckComponent implements OnInit {
  isCollapsed = true;
  constructor() { }
  @Input()
  healthCheckResult: HealthReportEntry;
  ngOnInit() {
  }

}
