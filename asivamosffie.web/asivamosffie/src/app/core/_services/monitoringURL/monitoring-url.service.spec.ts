import { TestBed } from '@angular/core/testing';

import { MonitoringURLService } from './monitoring-url.service';

describe('MonitoringURLService', () => {
  let service: MonitoringURLService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MonitoringURLService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
