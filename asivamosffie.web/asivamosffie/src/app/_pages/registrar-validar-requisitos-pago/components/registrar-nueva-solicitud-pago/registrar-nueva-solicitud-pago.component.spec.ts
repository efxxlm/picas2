import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarNuevaSolicitudPagoComponent } from './registrar-nueva-solicitud-pago.component';

describe('RegistrarNuevaSolicitudPagoComponent', () => {
  let component: RegistrarNuevaSolicitudPagoComponent;
  let fixture: ComponentFixture<RegistrarNuevaSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarNuevaSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarNuevaSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
