import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormEditValidarSolicitudValidfspComponent } from './form-edit-validar-solicitud-validfsp.component';

describe('FormEditValidarSolicitudValidfspComponent', () => {
  let component: FormEditValidarSolicitudValidfspComponent;
  let fixture: ComponentFixture<FormEditValidarSolicitudValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormEditValidarSolicitudValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormEditValidarSolicitudValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
