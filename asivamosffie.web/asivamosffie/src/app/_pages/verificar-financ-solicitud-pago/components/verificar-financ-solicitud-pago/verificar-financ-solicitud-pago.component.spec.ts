import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarFinancSolicitudPagoComponent } from './verificar-financ-solicitud-pago.component';

describe('VerificarFinancSolicitudPagoComponent', () => {
  let component: VerificarFinancSolicitudPagoComponent;
  let fixture: ComponentFixture<VerificarFinancSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarFinancSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarFinancSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
