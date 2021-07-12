import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDescripcionFacturaComponent } from './form-descripcion-factura.component';

describe('FormDescripcionFacturaComponent', () => {
  let component: FormDescripcionFacturaComponent;
  let fixture: ComponentFixture<FormDescripcionFacturaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDescripcionFacturaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDescripcionFacturaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
