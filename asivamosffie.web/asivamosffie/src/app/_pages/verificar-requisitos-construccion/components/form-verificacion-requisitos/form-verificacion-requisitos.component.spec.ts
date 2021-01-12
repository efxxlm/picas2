import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormVerificacionRequisitosComponent } from './form-verificacion-requisitos.component';

describe('FormVerificacionRequisitosComponent', () => {
  let component: FormVerificacionRequisitosComponent;
  let fixture: ComponentFixture<FormVerificacionRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormVerificacionRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormVerificacionRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
