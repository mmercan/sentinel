import { TestBed } from '@angular/core/testing';

import { ImageClassfierService } from './image-classfier.service';

describe('ImageClassfierService', () => {
  let service: ImageClassfierService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImageClassfierService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
