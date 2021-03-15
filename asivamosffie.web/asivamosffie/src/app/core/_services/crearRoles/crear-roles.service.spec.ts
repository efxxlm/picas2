import { TestBed } from '@angular/core/testing';

import { CrearRolesService } from './crear-roles.service';

describe('CrearRolesService', () => {
  let service: CrearRolesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CrearRolesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
