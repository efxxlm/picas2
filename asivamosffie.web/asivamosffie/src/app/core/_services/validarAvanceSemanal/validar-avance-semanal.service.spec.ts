import { TestBed } from '@angular/core/testing';

import { ValidarAvanceSemanalService } from './validar-avance-semanal.service';

describe('ValidarAvanceSemanalService', () => {
  let service: ValidarAvanceSemanalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ValidarAvanceSemanalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
