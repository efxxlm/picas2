import { TestBed } from '@angular/core/testing';

import { GestionarParametricasService } from './gestionar-parametricas.service';

describe('GestionarParametricasService', () => {
  let service: GestionarParametricasService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GestionarParametricasService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
