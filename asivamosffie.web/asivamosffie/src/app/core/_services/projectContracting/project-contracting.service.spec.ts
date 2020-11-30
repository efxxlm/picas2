import { TestBed } from '@angular/core/testing';

import { ProjectContractingService } from './project-contracting.service';

describe('ProjectContractingService', () => {
  let service: ProjectContractingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectContractingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
