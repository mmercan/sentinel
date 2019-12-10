import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { AppConfig, authenticationType, logLevel } from '../../app.config';
import { HealthcheckDataStoreSetService } from '../../shared/data-store/healthcheck-data-store/healthcheck-data-store-set.service';
import { HealthcheckDataStoreService } from '../../shared/data-store/healthcheck-data-store/healthcheck-data-store.service';
import { IHealthReport, IHealthReportEntry } from '../../shared/data-store/healthcheck-data-store/interfaces/health-report';

@Component({
  selector: 'app-prog-dashboard',
  templateUrl: './prog-dashboard.component.html',
  styleUrls: ['./prog-dashboard.component.scss'],
})
export class ProgDashboardComponent implements OnInit, OnDestroy {
  programname = '';
  envname = '';
  reportSet: IHealthReport[]; // = { duration: null, results: [], status: null };
  routeparamsSubscription: Subscription;
  healthcheckDataStoreSetSubscription: Subscription;
  GetHealthCheckUrlsSubscription: Subscription;
  constructor(
    private route: ActivatedRoute, private healthcheckDataStoreSetService: HealthcheckDataStoreSetService, private appConfig: AppConfig) {
  }

  ngOnInit() {
    this.routeparamsSubscription = this.route.params.subscribe((params) => {
      this.programname = params['programname'];
      this.envname = params['envname'];
      this.healthcheckDataStoreSetService.clear();
      this.healthcheckDataStoreSetSubscription = this.healthcheckDataStoreSetService.dataset.subscribe(
        (data) => {
          console.log('success', data);
          this.reportSet = data;
        },
        (error) => {
          console.log('oops', error);
          const blah = '';
        },
      );

      this.GetHealthCheckUrlsSubscription = this.healthcheckDataStoreSetService
        .GetHealthCheckUrls(this.programname, this.envname).subscribe((urls) => {
          if (urls && urls.length) {
            urls.forEach((element) => {
              this.healthcheckDataStoreSetService.AddHealthCheck(element.isaliveandwell, element.name);
            });
          }
        });
    });

  }
  ngOnDestroy(): void {
    if (this.routeparamsSubscription) { this.routeparamsSubscription.unsubscribe(); }
    if (this.GetHealthCheckUrlsSubscription) { this.GetHealthCheckUrlsSubscription.unsubscribe(); }
    if (this.healthcheckDataStoreSetSubscription) { this.healthcheckDataStoreSetSubscription.unsubscribe(); }
  }

}
