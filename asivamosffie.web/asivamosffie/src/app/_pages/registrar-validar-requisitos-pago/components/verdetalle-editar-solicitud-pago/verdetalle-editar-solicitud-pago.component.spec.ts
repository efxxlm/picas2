import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleEditarSolicitudPagoComponent } from './verdetalle-editar-solicitud-pago.component';

describe('VerdetalleEditarSolicitudPagoComponent', () => {
  let component: VerdetalleEditarSolicitudPagoComponent;
  let fixture: ComponentFixture<VerdetalleEditarSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleEditarSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleEditarSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
