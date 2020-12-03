import { TestBed } from '@angular/core/testing';

import { FaseUnoVerificarPreconstruccionService } from './fase-uno-verificar-preconstruccion.service';

describe('FaseUnoVerificarPreconstruccionService', () => {
  let service: FaseUnoVerificarPreconstruccionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FaseUnoVerificarPreconstruccionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
