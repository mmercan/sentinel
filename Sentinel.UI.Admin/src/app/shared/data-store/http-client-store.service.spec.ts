import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpClientStoreService } from './http-client-store.service';
import { Product } from './product-data-store/Interfaces/Production';

describe('HttpClientStoreService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      // providers: [HttpClientStoreService]
    });
  });

  it('should be created', inject([HttpClient], (httpclient: HttpClient) => {
    const service = new HttpClientStoreService(httpclient, 'https://www.google.com/', 'key', 1);
    expect(service).toBeTruthy();
  }));



});
