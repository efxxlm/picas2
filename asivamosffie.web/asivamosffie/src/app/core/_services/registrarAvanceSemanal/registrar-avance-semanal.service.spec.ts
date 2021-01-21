import { TestBed } from '@angular/core/testing';

import { RegistrarAvanceSemanalService } from './registrar-avance-semanal.service';

describe('RegistrarAvanceSemanalService', () => {
  let service: RegistrarAvanceSemanalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegistrarAvanceSemanalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
