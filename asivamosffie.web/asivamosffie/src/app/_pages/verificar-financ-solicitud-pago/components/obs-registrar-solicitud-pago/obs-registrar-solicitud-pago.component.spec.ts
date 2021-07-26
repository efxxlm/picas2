import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsRegistrarSolicitudPagoComponent } from './obs-registrar-solicitud-pago.component';

describe('ObsRegistrarSolicitudPagoComponent', () => {
  let component: ObsRegistrarSolicitudPagoComponent;
  let fixture: ComponentFixture<ObsRegistrarSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsRegistrarSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsRegistrarSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
