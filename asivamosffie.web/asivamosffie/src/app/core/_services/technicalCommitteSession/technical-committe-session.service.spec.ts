import { TestBed } from '@angular/core/testing';

import { TechnicalCommitteSessionService } from './technical-committe-session.service';

describe('TechnicalCommitteSessionService', () => {
  let service: TechnicalCommitteSessionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TechnicalCommitteSessionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
