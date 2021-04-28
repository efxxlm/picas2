import { TestBed } from '@angular/core/testing';

import { FinancialBalanceService } from './financial-balance.service';

describe('FinancialBalanceService', () => {
  let service: FinancialBalanceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FinancialBalanceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
