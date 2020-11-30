import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormInterventoriaVerificacionRequisitosComponent } from './form-interventoria-verificacion-requisitos.component';

describe('FormInterventoriaVerificacionRequisitosComponent', () => {
  let component: FormInterventoriaVerificacionRequisitosComponent;
  let fixture: ComponentFixture<FormInterventoriaVerificacionRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormInterventoriaVerificacionRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormInterventoriaVerificacionRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
