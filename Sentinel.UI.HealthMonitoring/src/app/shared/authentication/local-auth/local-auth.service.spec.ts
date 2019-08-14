import { TestBed, inject } from '@angular/core/testing';

import { LocalAuthService } from './local-auth.service';

import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppConfig, authenticationType, logLevel } from '../../../app.config';
import { NotificationService } from '../../notification/notification.service';
import { AdalService } from '../../authentication/adal-auth/adal.service';

describe('LocalAuthService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, HttpClientModule, RouterModule.forRoot([])],
      providers: [NotificationService, AdalService, AppConfig, LocalAuthService]
    });
  });

  it('should be created', inject([LocalAuthService], (service: LocalAuthService) => {
    expect(service).toBeTruthy();
  }));
});
