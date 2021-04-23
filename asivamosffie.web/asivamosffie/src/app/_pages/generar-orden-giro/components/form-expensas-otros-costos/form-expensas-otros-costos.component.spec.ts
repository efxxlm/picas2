import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormExpensasOtrosCostosComponent } from './form-expensas-otros-costos.component';

describe('FormExpensasOtrosCostosComponent', () => {
  let component: FormExpensasOtrosCostosComponent;
  let fixture: ComponentFixture<FormExpensasOtrosCostosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormExpensasOtrosCostosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormExpensasOtrosCostosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
