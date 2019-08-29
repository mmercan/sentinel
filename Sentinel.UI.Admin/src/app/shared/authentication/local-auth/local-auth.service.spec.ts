import { TestBed, inject } from '@angular/core/testing';

import { LocalAuthService } from './local-auth.service';

import { AppConfig } from '../../../app.config';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NotificationService } from '../../notification/notification.service';
import { AdalService } from '../adal-auth/adal.service';

describe('LocalAuthService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [LocalAuthService, AppConfig, NotificationService, AdalService]
    });
  });

  it('should be created', inject([LocalAuthService], (service: LocalAuthService) => {
    expect(service).toBeTruthy();
  }));
});
