import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesReportPagoRprComponent } from './observaciones-report-pago-rpr.component';

describe('ObservacionesReportPagoRprComponent', () => {
  let component: ObservacionesReportPagoRprComponent;
  let fixture: ComponentFixture<ObservacionesReportPagoRprComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesReportPagoRprComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesReportPagoRprComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
