import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarBalanceGbftrecComponent } from './validar-balance-gbftrec.component';

describe('ValidarBalanceGbftrecComponent', () => {
  let component: ValidarBalanceGbftrecComponent;
  let fixture: ComponentFixture<ValidarBalanceGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarBalanceGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarBalanceGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
