import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormEditAutorizarSolicitudComponent } from './form-edit-autorizar-solicitud.component';

describe('FormEditAutorizarSolicitudComponent', () => {
  let component: FormEditAutorizarSolicitudComponent;
  let fixture: ComponentFixture<FormEditAutorizarSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormEditAutorizarSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormEditAutorizarSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
