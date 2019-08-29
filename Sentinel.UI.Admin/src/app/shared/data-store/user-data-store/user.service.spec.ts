import { TestBed, inject } from '@angular/core/testing';
import { ProductService } from './user.service';
import { AppConfig } from '../../../app.config';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NotificationService } from '../../notification/notification.service';
import { AdalService } from '../../authentication/adal-auth/adal.service';
describe('ProductService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [ProductService, AppConfig, AdalService, NotificationService]
    });
  });

  it('should be created', inject([ProductService], (service: ProductService) => {
    expect(service).toBeTruthy();
  }));
});
