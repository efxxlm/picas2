import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDatosFacturaComponent } from './form-datos-factura.component';

describe('FormDatosFacturaComponent', () => {
  let component: FormDatosFacturaComponent;
  let fixture: ComponentFixture<FormDatosFacturaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDatosFacturaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDatosFacturaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
