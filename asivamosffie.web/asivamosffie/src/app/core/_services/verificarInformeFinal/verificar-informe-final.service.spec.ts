import { TestBed } from '@angular/core/testing';

import { VerificarInformeFinalService } from './verificar-informe-final.service';

describe('VerificarInformeFinalService', () => {
  let service: VerificarInformeFinalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VerificarInformeFinalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
