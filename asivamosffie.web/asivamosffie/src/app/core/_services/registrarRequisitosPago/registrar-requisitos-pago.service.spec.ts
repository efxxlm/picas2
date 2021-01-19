import { TestBed } from '@angular/core/testing';

import { RegistrarRequisitosPagoService } from './registrar-requisitos-pago.service';

describe('RegistrarRequisitosPagoService', () => {
  let service: RegistrarRequisitosPagoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegistrarRequisitosPagoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
