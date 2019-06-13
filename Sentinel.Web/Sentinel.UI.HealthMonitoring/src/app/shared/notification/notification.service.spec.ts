import { TestBed, inject } from '@angular/core/testing';

import { NotificationService } from './notification.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppConfig, authenticationType, logLevel } from '../../app.config';
import { AdalService } from '../authentication/adal-auth/adal.service';


describe('NotificationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, FormsModule],
      providers: [NotificationService]
    });
  });

  it('should be created', inject([NotificationService], (service: NotificationService) => {
    expect(service).toBeTruthy();
  }));
});
