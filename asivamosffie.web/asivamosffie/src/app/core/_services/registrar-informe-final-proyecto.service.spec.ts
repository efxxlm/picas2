import { TestBed } from '@angular/core/testing';

import { RegistrarInformeFinalProyectoService } from './registrar-informe-final-proyecto.service';

describe('RegistrarInformeFinalProyectoService', () => {
  let service: RegistrarInformeFinalProyectoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegistrarInformeFinalProyectoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
