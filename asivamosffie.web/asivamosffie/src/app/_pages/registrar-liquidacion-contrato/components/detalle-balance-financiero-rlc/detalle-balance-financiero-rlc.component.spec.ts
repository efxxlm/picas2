import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleBalanceFinancieroRlcComponent } from './detalle-balance-financiero-rlc.component';

describe('DetalleBalanceFinancieroRlcComponent', () => {
  let component: DetalleBalanceFinancieroRlcComponent;
  let fixture: ComponentFixture<DetalleBalanceFinancieroRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleBalanceFinancieroRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleBalanceFinancieroRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
