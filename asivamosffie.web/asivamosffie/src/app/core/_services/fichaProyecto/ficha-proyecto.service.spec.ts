import { TestBed } from '@angular/core/testing';

import { FichaProyectoService } from './ficha-proyecto.service';

describe('FichaProyectoService', () => {
  let service: FichaProyectoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FichaProyectoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
