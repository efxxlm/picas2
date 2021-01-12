import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDetallarSolicitudNovedadComponent } from './form-detallar-solicitud-novedad.component';

describe('FormDetallarSolicitudNovedadComponent', () => {
  let component: FormDetallarSolicitudNovedadComponent;
  let fixture: ComponentFixture<FormDetallarSolicitudNovedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDetallarSolicitudNovedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDetallarSolicitudNovedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
