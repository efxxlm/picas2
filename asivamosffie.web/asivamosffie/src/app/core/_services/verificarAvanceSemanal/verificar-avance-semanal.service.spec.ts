import { TestBed } from '@angular/core/testing';

import { VerificarAvanceSemanalService } from './verificar-avance-semanal.service';

describe('VerificarAvanceSemanalService', () => {
  let service: VerificarAvanceSemanalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VerificarAvanceSemanalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
