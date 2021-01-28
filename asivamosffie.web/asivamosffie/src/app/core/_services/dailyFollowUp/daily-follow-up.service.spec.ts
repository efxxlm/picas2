import { TestBed } from '@angular/core/testing';

import { FollowUpDailyService } from './follow-up-daily.service';

describe('FollowUpDailyService', () => {
  let service: FollowUpDailyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FollowUpDailyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
