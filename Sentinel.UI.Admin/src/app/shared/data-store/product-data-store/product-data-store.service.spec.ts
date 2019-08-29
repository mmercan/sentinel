import { TestBed, inject } from '@angular/core/testing';
import { ProductDataStoreService } from './product-data-store.service';

import { AppConfig } from '../../../app.config';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NotificationService } from '../../notification/notification.service';
import { AdalService } from '../../authentication/adal-auth/adal.service';

describe('ProductDataStoreService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [ProductDataStoreService, AppConfig]
    });
  });

  it('should be created', inject([ProductDataStoreService], (service: ProductDataStoreService) => {
    expect(service).toBeTruthy();
  }));
});
