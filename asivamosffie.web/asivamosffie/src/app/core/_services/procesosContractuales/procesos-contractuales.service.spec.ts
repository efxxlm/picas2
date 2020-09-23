import { TestBed } from '@angular/core/testing';

import { ProcesosContractualesService } from './procesos-contractuales.service';

describe('ProcesosContractualesService', () => {
  let service: ProcesosContractualesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProcesosContractualesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
