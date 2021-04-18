import { TestBed } from '@angular/core/testing';

import { GuardadoParcialAvanceSemanalService } from './guardado-parcial-avance-semanal.service';

describe('GuardadoParcialAvanceSemanalService', () => {
  let service: GuardadoParcialAvanceSemanalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GuardadoParcialAvanceSemanalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
