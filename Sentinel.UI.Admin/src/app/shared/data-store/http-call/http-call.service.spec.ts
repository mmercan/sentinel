import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpCallService } from './http-call.service';
import { AppConfig } from '../../../app.config';
import { NotificationService } from '../../notification/notification.service';
describe('HttpCallService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [HttpCallService, NotificationService, AppConfig]
    });
  });

  it('should be created', inject([HttpCallService], (service: HttpCallService) => {
    expect(service).toBeTruthy();
  }));
});
