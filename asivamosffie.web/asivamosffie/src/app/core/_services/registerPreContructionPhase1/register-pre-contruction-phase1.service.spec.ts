import { TestBed } from '@angular/core/testing';

import { RegisterPreContructionPhase1Service } from './register-pre-contruction-phase1.service';

describe('RegisterPreContructionPhase1Service', () => {
  let service: RegisterPreContructionPhase1Service;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegisterPreContructionPhase1Service);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
