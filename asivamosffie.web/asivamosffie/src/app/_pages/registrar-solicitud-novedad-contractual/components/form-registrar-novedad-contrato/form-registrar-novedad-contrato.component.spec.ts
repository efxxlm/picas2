import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarNovedadContratoComponent } from './form-registrar-novedad-contrato.component';

describe('FormRegistrarNovedadContratoComponent', () => {
  let component: FormRegistrarNovedadContratoComponent;
  let fixture: ComponentFixture<FormRegistrarNovedadContratoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarNovedadContratoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarNovedadContratoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
