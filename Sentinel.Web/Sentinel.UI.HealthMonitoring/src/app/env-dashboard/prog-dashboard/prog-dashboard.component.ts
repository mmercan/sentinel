import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HealthcheckDataStoreService } from '../../shared/data-store/healthcheck-data-store/healthcheck-data-store.service';
import { HealthcheckDataStoreSetService } from '../../shared/data-store/healthcheck-data-store/healthcheck-data-store-set.service';
import { HealthReport, HealthReportEntry } from '../../shared/data-store/healthcheck-data-store/interfaces/health-report';
import { AppConfig, authenticationType, logLevel } from '../../app.config';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-prog-dashboard',
  templateUrl: './prog-dashboard.component.html',
  styleUrls: ['./prog-dashboard.component.scss']
})
export class ProgDashboardComponent implements OnInit, OnDestroy {
  programname = '';
  envname = '';
  reportSet: HealthReport[]; // = { duration: null, results: [], status: null };
  routeparamsSubscription: Subscription;
  healthcheckDataStoreSetSubscription: Subscription;
  GetHealthCheckUrlsSubscription: Subscription;
  constructor(private route: ActivatedRoute, private healthcheckDataStoreSetService: HealthcheckDataStoreSetService
    , private healthDataService: HealthcheckDataStoreService, private appConfig: AppConfig) {
  }

  ngOnInit() {
    this.routeparamsSubscription = this.route.params.subscribe(params => {
      this.programname = params['programname'];
      this.envname = params['envname'];

      this.healthcheckDataStoreSetSubscription = this.healthcheckDataStoreSetService.dataset.subscribe(
        data => {
          console.log('success', data);
          // if (data && data.status) {
          this.reportSet = data;
          // }
        },
        error => {
          console.log('oops', error);
          const blah = '';
        }

      );
      // this.healthDataService.getAll(this.appConfig.config.HealthCheck.baseUrl + 'health/isaliveandwell', 'HealthMonitoring Api');
      //  this.appConfig.config.HealthCheck.urls
      this.GetHealthCheckUrlsSubscription = this.healthcheckDataStoreSetService
        .GetHealthCheckUrls(this.programname, this.envname).subscribe(urls => {
          if (urls && urls.length) {
            urls.forEach(element => {
              this.healthcheckDataStoreSetService.AddHealthCheck(element.isaliveandwell, element.name);
            });
          }
        });
      // this.appConfig.config.HealthCheck.urls.forEach(element => {
      //   this.healthcheckDataStoreSetService.AddHealthCheck(element.isaliveandwell, element.name);
      // });

    });

  }
  ngOnDestroy(): void {
    if (this.routeparamsSubscription) { this.routeparamsSubscription.unsubscribe(); }
    if (this.GetHealthCheckUrlsSubscription) { this.GetHealthCheckUrlsSubscription.unsubscribe(); }
    if (this.healthcheckDataStoreSetSubscription) { this.healthcheckDataStoreSetSubscription.unsubscribe(); }
  }

}
