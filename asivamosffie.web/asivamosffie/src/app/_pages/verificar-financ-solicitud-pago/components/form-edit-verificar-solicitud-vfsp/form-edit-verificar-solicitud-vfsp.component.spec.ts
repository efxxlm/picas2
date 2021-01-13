import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormEditVerificarSolicitudVfspComponent } from './form-edit-verificar-solicitud-vfsp.component';

describe('FormEditVerificarSolicitudVfspComponent', () => {
  let component: FormEditVerificarSolicitudVfspComponent;
  let fixture: ComponentFixture<FormEditVerificarSolicitudVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormEditVerificarSolicitudVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormEditVerificarSolicitudVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
