import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ConfigApiService } from './config-api.service';
import { NotificationService } from '../../notification/notification.service';
import { AppConfig } from '../../../app.config';

describe('ConfigApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [ConfigApiService, NotificationService, AppConfig]
    });
  });

  it('should be created', inject([ConfigApiService], (service: ConfigApiService) => {
    expect(service).toBeTruthy();
  }));
});
