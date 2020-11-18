import { TestBed } from '@angular/core/testing';

import { ProgramacionPersonalObraService } from './programacion-personal-obra.service';

describe('ProgramacionPersonalObraService', () => {
  let service: ProgramacionPersonalObraService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProgramacionPersonalObraService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
