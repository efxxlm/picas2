import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AmortizacionPagoComponent } from './amortizacion-pago.component';

describe('AmortizacionPagoComponent', () => {
  let component: AmortizacionPagoComponent;
  let fixture: ComponentFixture<AmortizacionPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AmortizacionPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AmortizacionPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
