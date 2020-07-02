import { TestBed } from '@angular/core/testing';

import { CofinanciacionService } from './cofinanciacion.service';

describe('CofinanciacionService', () => {
  let service: CofinanciacionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CofinanciacionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
