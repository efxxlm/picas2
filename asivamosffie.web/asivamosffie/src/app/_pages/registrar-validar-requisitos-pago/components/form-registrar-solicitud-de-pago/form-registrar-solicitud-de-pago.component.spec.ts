import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarSolicitudDePagoComponent } from './form-registrar-solicitud-de-pago.component';

describe('FormRegistrarSolicitudDePagoComponent', () => {
  let component: FormRegistrarSolicitudDePagoComponent;
  let fixture: ComponentFixture<FormRegistrarSolicitudDePagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarSolicitudDePagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarSolicitudDePagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
