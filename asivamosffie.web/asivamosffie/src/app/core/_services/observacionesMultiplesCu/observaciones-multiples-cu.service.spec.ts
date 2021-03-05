import { TestBed } from '@angular/core/testing';

import { ObservacionesMultiplesCuService } from './observaciones-multiples-cu.service';

describe('ObservacionesMultiplesCuService', () => {
  let service: ObservacionesMultiplesCuService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ObservacionesMultiplesCuService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
