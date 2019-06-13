import { TestBed } from '@angular/core/testing';

import { HealthcheckDataStoreService } from './healthcheck-data-store.service';


import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppConfig, authenticationType, logLevel } from '../../../app.config';
import { NotificationService } from '../../notification/notification.service';
import { AdalService } from '../../authentication/adal-auth/adal.service';

describe('HealthcheckDataStoreService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [CommonModule, HttpClientModule],
    providers: [NotificationService, AdalService, AppConfig, HealthcheckDataStoreService]
  }));

  it('should be created', () => {
    const service: HealthcheckDataStoreService = TestBed.get(HealthcheckDataStoreService);
    expect(service).toBeTruthy();
  });
});
