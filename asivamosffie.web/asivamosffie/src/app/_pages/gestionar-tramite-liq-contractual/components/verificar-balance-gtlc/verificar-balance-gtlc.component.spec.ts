import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarBalanceGtlcComponent } from './verificar-balance-gtlc.component';

describe('VerificarBalanceGtlcComponent', () => {
  let component: VerificarBalanceGtlcComponent;
  let fixture: ComponentFixture<VerificarBalanceGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarBalanceGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarBalanceGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
