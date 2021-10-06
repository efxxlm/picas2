import { TestBed } from '@angular/core/testing';

import { ReleaseBalanceService } from './release-balance.service';

describe('ReleaseBalanceService', () => {
  let service: ReleaseBalanceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReleaseBalanceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
