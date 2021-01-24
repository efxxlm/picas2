import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormAprobarSolicitudComponent } from './form-aprobar-solicitud.component';

describe('FormAprobarSolicitudComponent', () => {
  let component: FormAprobarSolicitudComponent;
  let fixture: ComponentFixture<FormAprobarSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormAprobarSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormAprobarSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
