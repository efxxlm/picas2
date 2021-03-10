import { TestBed } from '@angular/core/testing';

import { ValidarCumplimientoInformeFinalService } from './validar-cumplimiento-informe-final.service';

describe('ValidarCumplimientoInformeFinalService', () => {
  let service: ValidarCumplimientoInformeFinalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ValidarCumplimientoInformeFinalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
