import { TestBed, inject } from '@angular/core/testing';

import { CommonLocalStoreService } from './common-local-store.service';

describe('CommonLocalStoreService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CommonLocalStoreService]
    });
  });

  it('should be created', inject([CommonLocalStoreService], (service: CommonLocalStoreService) => {
    expect(service).toBeTruthy();
  }));
});
