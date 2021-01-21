import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormAutorizarSolicitudComponent } from './form-autorizar-solicitud.component';

describe('FormAutorizarSolicitudComponent', () => {
  let component: FormAutorizarSolicitudComponent;
  let fixture: ComponentFixture<FormAutorizarSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormAutorizarSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormAutorizarSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
