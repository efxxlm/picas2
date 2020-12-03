import { TestBed } from '@angular/core/testing';

import { FaseUnoConstruccionService } from './fase-uno-construccion.service';

describe('FaseUnoConstruccionService', () => {
  let service: FaseUnoConstruccionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FaseUnoConstruccionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
