import { TestBed, inject } from '@angular/core/testing';

import { HttpCallService } from './http-call.service';

describe('HttpCallService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HttpCallService]
    });
  });

  it('should be created', inject([HttpCallService], (service: HttpCallService) => {
    expect(service).toBeTruthy();
  }));
});
