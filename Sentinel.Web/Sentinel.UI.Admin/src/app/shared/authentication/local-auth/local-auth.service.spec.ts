import { TestBed, inject } from '@angular/core/testing';

import { LocalAuthService } from './local-auth.service';

describe('LocalAuthService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LocalAuthService]
    });
  });

  it('should be created', inject([LocalAuthService], (service: LocalAuthService) => {
    expect(service).toBeTruthy();
  }));
});
