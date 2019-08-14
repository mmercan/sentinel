import { TestBed, inject } from '@angular/core/testing';

import { ProductDataStoreService } from './product-data-store.service';

import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppConfig, authenticationType, logLevel } from '../../../app.config';
import { NotificationService } from '../../notification/notification.service';
import { AdalService } from '../../authentication/adal-auth/adal.service';

describe('ProductDataStoreService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, HttpClientModule],
      providers: [NotificationService, AdalService, AppConfig, ProductDataStoreService]
    });
  });

  it('should be created', inject([ProductDataStoreService], (service: ProductDataStoreService) => {
    expect(service).toBeTruthy();
  }));
});
