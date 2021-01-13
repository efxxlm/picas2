import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesReportRendRprComponent } from './observaciones-report-rend-rpr.component';

describe('ObservacionesReportRendRprComponent', () => {
  let component: ObservacionesReportRendRprComponent;
  let fixture: ComponentFixture<ObservacionesReportRendRprComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesReportRendRprComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesReportRendRprComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
