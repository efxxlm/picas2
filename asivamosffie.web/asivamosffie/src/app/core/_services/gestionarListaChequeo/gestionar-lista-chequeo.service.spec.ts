import { TestBed } from '@angular/core/testing';

import { GestionarListaChequeoService } from './gestionar-lista-chequeo.service';

describe('GestionarListaChequeoService', () => {
  let service: GestionarListaChequeoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GestionarListaChequeoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
