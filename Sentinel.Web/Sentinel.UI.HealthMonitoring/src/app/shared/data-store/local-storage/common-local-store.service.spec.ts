import { TestBed, inject } from '@angular/core/testing';

import { CommonLocalStoreService } from './common-local-store.service';

describe('CommonLocalStoreService', () => {
  beforeEach(() => {
    // TestBed.configureTestingModule({
    //   providers: [CommonLocalStoreService]
    // });
  });

  // it('should be created', inject([CommonLocalStoreService], (service: CommonLocalStoreService<object>) => {
  //   expect(service).toBeTruthy();
  // }));

  it('should create an instance', () => {
    const service = new CommonLocalStoreService('blah');
    expect(service).toBeTruthy();
  });
});
