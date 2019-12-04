import { TestBed } from '@angular/core/testing';

import { PollyService } from './polly.service';

describe('PollyService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PollyService = TestBed.get(PollyService);
    expect(service).toBeTruthy();
  });
});
