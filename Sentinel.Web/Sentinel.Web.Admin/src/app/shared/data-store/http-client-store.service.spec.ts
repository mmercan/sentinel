import { TestBed, inject } from '@angular/core/testing';

import { HttpClientStoreService } from './http-client-store.service';

describe('HttpClientStoreService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HttpClientStoreService]
    });
  });

  it('should be created', inject([HttpClientStoreService], (service: HttpClientStoreService) => {
    expect(service).toBeTruthy();
  }));
});
