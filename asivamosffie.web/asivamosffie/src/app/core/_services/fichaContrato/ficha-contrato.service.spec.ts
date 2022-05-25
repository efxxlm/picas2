import { TestBed } from '@angular/core/testing';

import { FichaContratoService } from './ficha-contrato.service';

describe('FichaContratoService', () => {
  let service: FichaContratoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FichaContratoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
