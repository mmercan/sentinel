import { TestBed, inject } from '@angular/core/testing';

import { ProductDataStoreService } from './product-data-store.service';

describe('ProductDataStoreService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProductDataStoreService]
    });
  });

  it('should be created', inject([ProductDataStoreService], (service: ProductDataStoreService) => {
    expect(service).toBeTruthy();
  }));
});
