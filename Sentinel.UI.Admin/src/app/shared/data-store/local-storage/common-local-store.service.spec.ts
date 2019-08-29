import { TestBed, inject } from '@angular/core/testing';

import { CommonLocalStoreService } from './common-local-store.service';
import { Product } from '../product-data-store/Interfaces/Production';

describe('CommonLocalStoreService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CommonLocalStoreService]
    });
  });

  // it('should be created', inject([CommonLocalStoreService], (service: CommonLocalStoreService<Product>) => {
  //   expect(service).toBeTruthy();
  // }));
  it('should be created', () => {
    const store = new CommonLocalStoreService<Product>('mykey');
    expect(store).toBeTruthy();
  });

});
