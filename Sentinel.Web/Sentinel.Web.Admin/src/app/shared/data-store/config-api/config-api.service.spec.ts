import { TestBed, inject } from '@angular/core/testing';

import { ConfigApiService } from './config-api.service';

describe('ConfigApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ConfigApiService]
    });
  });

  it('should be created', inject([ConfigApiService], (service: ConfigApiService) => {
    expect(service).toBeTruthy();
  }));
});
