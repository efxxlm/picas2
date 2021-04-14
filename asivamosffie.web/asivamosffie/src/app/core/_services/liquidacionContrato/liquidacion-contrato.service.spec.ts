import { TestBed } from '@angular/core/testing';

import { LiquidacionContratoService } from './liquidacion-contrato.service';

describe('LiquidacionContratoService', () => {
  let service: LiquidacionContratoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LiquidacionContratoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
