import { TestBed, inject } from '@angular/core/testing';

import { HttpCallService } from './http-call.service';

import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppConfig, authenticationType, logLevel } from '../../../app.config';
import { NotificationService } from '../../notification/notification.service';
import { AdalService } from '../../authentication/adal-auth/adal.service';

describe('HttpCallService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, HttpClientModule],
      providers: [NotificationService, AdalService, AppConfig, HttpCallService]
    });
  });

  it('should be created', inject([HttpCallService], (service: HttpCallService) => {
    expect(service).toBeTruthy();
  }));
});
