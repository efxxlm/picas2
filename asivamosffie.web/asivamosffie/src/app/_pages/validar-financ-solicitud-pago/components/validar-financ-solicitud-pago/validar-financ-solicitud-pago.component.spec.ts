import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarFinancSolicitudPagoComponent } from './validar-financ-solicitud-pago.component';

describe('ValidarFinancSolicitudPagoComponent', () => {
  let component: ValidarFinancSolicitudPagoComponent;
  let fixture: ComponentFixture<ValidarFinancSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarFinancSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarFinancSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
