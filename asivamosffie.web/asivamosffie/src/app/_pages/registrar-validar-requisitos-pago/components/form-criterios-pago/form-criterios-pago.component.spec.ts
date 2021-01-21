import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCriteriosPagoComponent } from './form-criterios-pago.component';

describe('FormCriteriosPagoComponent', () => {
  let component: FormCriteriosPagoComponent;
  let fixture: ComponentFixture<FormCriteriosPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCriteriosPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCriteriosPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
