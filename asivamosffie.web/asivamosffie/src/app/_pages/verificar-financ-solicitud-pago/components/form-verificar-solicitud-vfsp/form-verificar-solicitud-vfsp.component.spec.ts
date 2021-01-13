import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormVerificarSolicitudVfspComponent } from './form-verificar-solicitud-vfsp.component';

describe('FormVerificarSolicitudVfspComponent', () => {
  let component: FormVerificarSolicitudVfspComponent;
  let fixture: ComponentFixture<FormVerificarSolicitudVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormVerificarSolicitudVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormVerificarSolicitudVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
