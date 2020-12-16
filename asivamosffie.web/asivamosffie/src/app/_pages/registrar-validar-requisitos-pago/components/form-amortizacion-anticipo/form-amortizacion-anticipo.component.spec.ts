import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormAmortizacionAnticipoComponent } from './form-amortizacion-anticipo.component';

describe('FormAmortizacionAnticipoComponent', () => {
  let component: FormAmortizacionAnticipoComponent;
  let fixture: ComponentFixture<FormAmortizacionAnticipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormAmortizacionAnticipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormAmortizacionAnticipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
