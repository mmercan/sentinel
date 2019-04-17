import { TestBed } from '@angular/core/testing';

import { HealthcheckDataStoreService } from './healthcheck-data-store.service';

describe('HealthcheckDataStoreService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HealthcheckDataStoreService = TestBed.get(HealthcheckDataStoreService);
    expect(service).toBeTruthy();
  });
});
