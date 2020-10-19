import { TestBed } from '@angular/core/testing';

import { PolizaGarantiaService } from './poliza-garantia.service';

describe('PolizaGarantiaService', () => {
  let service: PolizaGarantiaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PolizaGarantiaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
