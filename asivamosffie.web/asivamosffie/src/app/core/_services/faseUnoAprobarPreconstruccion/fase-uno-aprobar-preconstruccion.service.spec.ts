import { TestBed } from '@angular/core/testing';

import { FaseUnoAprobarPreconstruccionService } from './fase-uno-aprobar-preconstruccion.service';

describe('FaseUnoAprobarPreconstruccionService', () => {
  let service: FaseUnoAprobarPreconstruccionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FaseUnoAprobarPreconstruccionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
