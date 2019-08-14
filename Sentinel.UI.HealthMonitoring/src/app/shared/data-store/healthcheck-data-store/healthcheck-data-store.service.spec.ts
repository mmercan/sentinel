import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { AppConfig } from '../../../app.config';
import { AdalService } from '../../authentication/adal-auth/adal.service';
import { NotificationService } from '../../notification/notification.service';
import { HealthcheckDataStoreService } from './healthcheck-data-store.service';

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
