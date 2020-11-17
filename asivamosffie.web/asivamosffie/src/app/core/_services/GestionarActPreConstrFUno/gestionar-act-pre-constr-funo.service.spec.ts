import { TestBed } from '@angular/core/testing';

import { GestionarActPreConstrFUnoService } from './gestionar-act-pre-constr-funo.service';

describe('GestionarActPreConstrFUnoService', () => {
  let service: GestionarActPreConstrFUnoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GestionarActPreConstrFUnoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
