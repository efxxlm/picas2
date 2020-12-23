import { TestBed } from '@angular/core/testing';

import { ContractualControversyService } from './contractual-controversy.service';

describe('ContractualControversyService', () => {
  let service: ContractualControversyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContractualControversyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
