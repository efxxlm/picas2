import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleRegSolPagoValidfspComponent } from './detalle-reg-sol-pago-validfsp.component';

describe('DetalleRegSolPagoValidfspComponent', () => {
  let component: DetalleRegSolPagoValidfspComponent;
  let fixture: ComponentFixture<DetalleRegSolPagoValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleRegSolPagoValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleRegSolPagoValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
