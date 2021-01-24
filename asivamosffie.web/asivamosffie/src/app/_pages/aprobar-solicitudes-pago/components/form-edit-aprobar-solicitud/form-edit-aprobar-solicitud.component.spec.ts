import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormEditAprobarSolicitudComponent } from './form-edit-aprobar-solicitud.component';

describe('FormEditAprobarSolicitudComponent', () => {
  let component: FormEditAprobarSolicitudComponent;
  let fixture: ComponentFixture<FormEditAprobarSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormEditAprobarSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormEditAprobarSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
