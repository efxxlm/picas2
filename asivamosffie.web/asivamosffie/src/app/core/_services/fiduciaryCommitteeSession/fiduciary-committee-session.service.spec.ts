import { TestBed } from '@angular/core/testing';

import { FiduciaryCommitteeSessionService } from './fiduciary-committee-session.service';

describe('FiduciaryCommitteeSessionService', () => {
  let service: FiduciaryCommitteeSessionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FiduciaryCommitteeSessionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
