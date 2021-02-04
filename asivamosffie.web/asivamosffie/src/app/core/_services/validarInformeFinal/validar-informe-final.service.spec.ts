import { TestBed } from '@angular/core/testing';

import { ValidarInformeFinalService } from './validar-informe-final.service';

describe('ValidarInformeFinalService', () => {
  let service: ValidarInformeFinalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ValidarInformeFinalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
