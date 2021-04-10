import { TestBed } from '@angular/core/testing';

import { ActualUserGuard } from './actual-user.guard';

describe('ActualUserGuard', () => {
  let guard: ActualUserGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(ActualUserGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
