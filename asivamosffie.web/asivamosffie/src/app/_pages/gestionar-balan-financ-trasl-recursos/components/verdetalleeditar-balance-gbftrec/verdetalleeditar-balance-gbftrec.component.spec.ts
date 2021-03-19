import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleeditarBalanceGbftrecComponent } from './verdetalleeditar-balance-gbftrec.component';

describe('VerdetalleeditarBalanceGbftrecComponent', () => {
  let component: VerdetalleeditarBalanceGbftrecComponent;
  let fixture: ComponentFixture<VerdetalleeditarBalanceGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleeditarBalanceGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleeditarBalanceGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
