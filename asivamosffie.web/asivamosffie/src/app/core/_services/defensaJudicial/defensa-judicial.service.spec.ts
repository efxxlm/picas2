import { TestBed } from '@angular/core/testing';

import { DefensaJudicialService } from './defensa-judicial.service';

describe('DefensaJudicialService', () => {
  let service: DefensaJudicialService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DefensaJudicialService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
