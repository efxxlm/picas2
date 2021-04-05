import { TestBed } from '@angular/core/testing';

import { ActualizarPolizasService } from './actualizar-polizas.service';

describe('ActualizarPolizasService', () => {
  let service: ActualizarPolizasService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ActualizarPolizasService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
