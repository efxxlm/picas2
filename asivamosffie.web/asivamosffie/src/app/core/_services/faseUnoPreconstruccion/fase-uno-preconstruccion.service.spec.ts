import { TestBed } from '@angular/core/testing';

import { FaseUnoPreconstruccionService } from './fase-uno-preconstruccion.service';

describe('FaseUnoPreconstruccionService', () => {
  let service: FaseUnoPreconstruccionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FaseUnoPreconstruccionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
