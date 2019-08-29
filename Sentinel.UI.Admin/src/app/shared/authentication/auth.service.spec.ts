import { TestBed, inject } from '@angular/core/testing';
import { AuthService } from './auth.service';

import { AppConfig } from '../../app.config';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NotificationService } from '../notification/notification.service';
import { AdalService } from './adal-auth/adal.service';

describe('AuthService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [AuthService, AppConfig, NotificationService, AdalService]
    });
  });

  it('should be created', inject([AuthService], (service: AuthService) => {
    expect(service).toBeTruthy();
  }));
});
