import { TestBed } from '@angular/core/testing';

import { FuenteFinanciacionService } from './fuente-financiacion.service';

describe('FuenteFinanciacionService', () => {
  let service: FuenteFinanciacionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FuenteFinanciacionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
