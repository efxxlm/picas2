import { TestBed } from '@angular/core/testing';

import { CompromisosActasComiteService } from './compromisos-actas-comite.service';

describe('CompromisosActasComiteService', () => {
  let service: CompromisosActasComiteService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CompromisosActasComiteService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
