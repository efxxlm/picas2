import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleBalanceFinancGtlcComponent } from './detalle-balance-financ-gtlc.component';

describe('DetalleBalanceFinancGtlcComponent', () => {
  let component: DetalleBalanceFinancGtlcComponent;
  let fixture: ComponentFixture<DetalleBalanceFinancGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleBalanceFinancGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleBalanceFinancGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
