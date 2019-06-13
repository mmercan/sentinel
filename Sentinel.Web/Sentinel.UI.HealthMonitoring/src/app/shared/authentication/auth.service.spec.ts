import { TestBed, inject } from '@angular/core/testing';

import { AuthService } from './auth.service';

import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppConfig, authenticationType, logLevel } from '../../app.config';
import { NotificationService } from '../notification/notification.service';
import { AdalService } from '../authentication/adal-auth/adal.service';

describe('AuthService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, HttpClientModule, RouterModule.forRoot([])],
      providers: [AdalService, NotificationService, AppConfig, AuthService]
    });
  });

  it('should be created', inject([AuthService], (service: AuthService) => {
    expect(service).toBeTruthy();
  }));
});
