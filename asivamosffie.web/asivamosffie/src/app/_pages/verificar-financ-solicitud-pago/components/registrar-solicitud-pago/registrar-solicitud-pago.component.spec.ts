import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarSolicitudPagoComponent } from './registrar-solicitud-pago.component';

describe('RegistrarSolicitudPagoComponent', () => {
  let component: RegistrarSolicitudPagoComponent;
  let fixture: ComponentFixture<RegistrarSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
