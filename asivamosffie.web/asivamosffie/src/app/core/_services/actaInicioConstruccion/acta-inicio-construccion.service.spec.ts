import { TestBed } from '@angular/core/testing';

import { ActaInicioConstruccionService } from './acta-inicio-construccion.service';

describe('ActaInicioConstruccionService', () => {
  let service: ActaInicioConstruccionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ActaInicioConstruccionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
