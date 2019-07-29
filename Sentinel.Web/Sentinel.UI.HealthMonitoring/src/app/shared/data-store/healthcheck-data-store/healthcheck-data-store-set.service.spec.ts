import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { AppConfig } from '../../../app.config';
import { AdalService } from '../../authentication/adal-auth/adal.service';
import { NotificationService } from '../../notification/notification.service';
import { HealthcheckDataStoreSetService } from './healthcheck-data-store-set.service';

describe('HealthcheckDataStoreService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [CommonModule, HttpClientModule],
    providers: [NotificationService, AdalService, AppConfig, HealthcheckDataStoreSetService]
  }));

  it('should be created', () => {
    const service: HealthcheckDataStoreSetService = TestBed.get(HealthcheckDataStoreSetService);
    expect(service).toBeTruthy();
  });

  it('should be AddHealthCheck', () => {
    const service: HealthcheckDataStoreSetService = TestBed.get(HealthcheckDataStoreSetService);
    service.AddHealthCheck('https://comms.api.myrcan.com/health/isaliveandwell', 'Comms Api');
    //expect(service).toBeTruthy();
  });


});
