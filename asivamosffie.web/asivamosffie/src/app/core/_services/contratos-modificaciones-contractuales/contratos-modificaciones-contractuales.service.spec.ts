import { TestBed } from '@angular/core/testing';

import { ContratosModificacionesContractualesService } from './contratos-modificaciones-contractuales.service';

describe('ContratosModificacionesContractualesService', () => {
  let service: ContratosModificacionesContractualesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContratosModificacionesContractualesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
