import { TestBed } from '@angular/core/testing';

import { GestionarUsuariosService } from './gestionar-usuarios.service';

describe('GestionarUsuariosService', () => {
  let service: GestionarUsuariosService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GestionarUsuariosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
