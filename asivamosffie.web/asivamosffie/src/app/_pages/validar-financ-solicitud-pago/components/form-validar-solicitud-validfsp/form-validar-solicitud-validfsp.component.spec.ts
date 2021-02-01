import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidarSolicitudValidfspComponent } from './form-validar-solicitud-validfsp.component';

describe('FormValidarSolicitudValidfspComponent', () => {
  let component: FormValidarSolicitudValidfspComponent;
  let fixture: ComponentFixture<FormValidarSolicitudValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormValidarSolicitudValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormValidarSolicitudValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
