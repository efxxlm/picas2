import { TestBed } from '@angular/core/testing';

import { ActBeginService } from './act-begin.service';

describe('ActBeginService', () => {
  let service: ActBeginService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ActBeginService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
