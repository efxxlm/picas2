import { TestBed } from '@angular/core/testing';

import { RegisterProjectEtcService } from './register-project-etc.service';

describe('RegisterProjectEtcService', () => {
  let service: RegisterProjectEtcService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegisterProjectEtcService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
