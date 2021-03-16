import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarBalanceComponent } from './validar-balance.component';

describe('ValidarBalanceComponent', () => {
  let component: ValidarBalanceComponent;
  let fixture: ComponentFixture<ValidarBalanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarBalanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
