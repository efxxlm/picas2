import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormAmortizacionComponent } from './form-amortizacion.component';

describe('FormAmortizacionComponent', () => {
  let component: FormAmortizacionComponent;
  let fixture: ComponentFixture<FormAmortizacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormAmortizacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormAmortizacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
