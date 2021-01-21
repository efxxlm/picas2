import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleSolicitudPagoComponent } from './verdetalle-solicitud-pago.component';

describe('VerdetalleSolicitudPagoComponent', () => {
  let component: VerdetalleSolicitudPagoComponent;
  let fixture: ComponentFixture<VerdetalleSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
