import { TestBed } from '@angular/core/testing';

import { DisponibilidadPresupuestalService } from './disponibilidad-presupuestal.service';

describe('DisponibilidadPresupuestalService', () => {
  let service: DisponibilidadPresupuestalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DisponibilidadPresupuestalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
