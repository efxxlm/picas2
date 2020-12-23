import { TestBed } from '@angular/core/testing';

import { FaseDosAprobarConstruccionService } from './fase-dos-aprobar-construccion.service';

describe('FaseDosAprobarConstruccionService', () => {
  let service: FaseDosAprobarConstruccionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FaseDosAprobarConstruccionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
