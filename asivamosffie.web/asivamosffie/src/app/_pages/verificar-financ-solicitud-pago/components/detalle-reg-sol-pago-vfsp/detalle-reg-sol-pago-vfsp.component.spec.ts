import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleRegSolPagoVfspComponent } from './detalle-reg-sol-pago-vfsp.component';

describe('DetalleRegSolPagoVfspComponent', () => {
  let component: DetalleRegSolPagoVfspComponent;
  let fixture: ComponentFixture<DetalleRegSolPagoVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleRegSolPagoVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleRegSolPagoVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
