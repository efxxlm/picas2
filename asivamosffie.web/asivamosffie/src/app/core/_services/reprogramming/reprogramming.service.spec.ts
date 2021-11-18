import { TestBed } from '@angular/core/testing';

import { ReprogrammingService } from './reprogramming.service';

describe('ReprogrammingService', () => {
  let service: ReprogrammingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReprogrammingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
