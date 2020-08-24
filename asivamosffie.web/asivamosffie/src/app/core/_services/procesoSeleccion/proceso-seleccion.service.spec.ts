import { TestBed } from '@angular/core/testing';

import { ProcesoSeleccionService } from './proceso-seleccion.service';

describe('ProcesoSeleccionService', () => {
  let service: ProcesoSeleccionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProcesoSeleccionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
