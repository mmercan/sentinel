import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HealthcheckDataStoreService } from '../../shared/data-store/healthcheck-data-store/healthcheck-data-store.service';
import { HealthReport, HealthReportEntry } from '../../shared/data-store/healthcheck-data-store/interfaces/health-report';
import { AppConfig, authenticationType, logLevel } from '../../app.config';
@Component({
  selector: 'app-prog-dashboard',
  templateUrl: './prog-dashboard.component.html',
  styleUrls: ['./prog-dashboard.component.scss']
})
export class ProgDashboardComponent implements OnInit {
  programname = '';
  envname = '';
  report: HealthReport = { duration: null, results: [], status: null };
  constructor(private route: ActivatedRoute, private healthDataService: HealthcheckDataStoreService, private appConfig: AppConfig) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.programname = params['programname'];
      this.envname = params['envname'];

      this.healthDataService.dataset.subscribe(
        data => {
          console.log('success', data);
          if (data && data.status) {
            this.report = data;
          }
        },
        error => {
          console.log('oops', error);
        }

      );
      this.healthDataService.getAll(this.appConfig.config.HealthCheck.baseUrl + 'health/isaliveandwell', 'HealthMonitoring Api');
    });

  }

}
