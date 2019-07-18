// import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { inject, TestBed } from '@angular/core/testing';

import { ConfigApiService } from './config-api.service';

import { AppConfig, authenticationType, logLevel } from '../../../app.config';
import { NotificationService } from '../../notification/notification.service';

describe('ConfigApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [NotificationService, AppConfig, ConfigApiService],
    });
  });

  it('should be created', inject([ConfigApiService], (service: ConfigApiService) => {
    expect(service).toBeTruthy();
  }));
});
