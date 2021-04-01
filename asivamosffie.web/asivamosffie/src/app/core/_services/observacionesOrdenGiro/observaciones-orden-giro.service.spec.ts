import { TestBed } from '@angular/core/testing';

import { ObservacionesOrdenGiroService } from './observaciones-orden-giro.service';

describe('ObservacionesOrdenGiroService', () => {
  let service: ObservacionesOrdenGiroService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ObservacionesOrdenGiroService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
