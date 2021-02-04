import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleBalanceGbftrecComponent } from './detalle-balance-gbftrec.component';

describe('DetalleBalanceGbftrecComponent', () => {
  let component: DetalleBalanceGbftrecComponent;
  let fixture: ComponentFixture<DetalleBalanceGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleBalanceGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleBalanceGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
