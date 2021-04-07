import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormObservacionBalanceComponent } from './form-observacion-balance.component';

describe('FormObservacionBalanceComponent', () => {
  let component: FormObservacionBalanceComponent;
  let fixture: ComponentFixture<FormObservacionBalanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormObservacionBalanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormObservacionBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
