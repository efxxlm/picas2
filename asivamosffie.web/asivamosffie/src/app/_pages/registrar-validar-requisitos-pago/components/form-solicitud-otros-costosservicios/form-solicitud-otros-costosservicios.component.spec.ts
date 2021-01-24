import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSolicitudOtrosCostosserviciosComponent } from './form-solicitud-otros-costosservicios.component';

describe('FormSolicitudOtrosCostosserviciosComponent', () => {
  let component: FormSolicitudOtrosCostosserviciosComponent;
  let fixture: ComponentFixture<FormSolicitudOtrosCostosserviciosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormSolicitudOtrosCostosserviciosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormSolicitudOtrosCostosserviciosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
