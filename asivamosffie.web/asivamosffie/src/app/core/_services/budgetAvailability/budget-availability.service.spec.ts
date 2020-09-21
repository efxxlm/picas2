import { TestBed } from '@angular/core/testing';

import { BudgetAvailabilityService } from './budget-availability.service';

describe('BudgetAvailabilityService', () => {
  let service: BudgetAvailabilityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BudgetAvailabilityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
