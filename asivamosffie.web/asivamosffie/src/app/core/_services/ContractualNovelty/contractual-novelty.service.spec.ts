import { TestBed } from '@angular/core/testing';

import { ContractualNoveltyService } from './contractual-novelty.service';

describe('ContractualNoveltyService', () => {
  let service: ContractualNoveltyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContractualNoveltyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
