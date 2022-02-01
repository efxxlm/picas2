import { TestBed } from '@angular/core/testing';

import { ValidacionesLineaPagoService } from './validaciones-linea-pago.service';

describe('ValidacionesLineaPagoService', () => {
  let service: ValidacionesLineaPagoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ValidacionesLineaPagoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
