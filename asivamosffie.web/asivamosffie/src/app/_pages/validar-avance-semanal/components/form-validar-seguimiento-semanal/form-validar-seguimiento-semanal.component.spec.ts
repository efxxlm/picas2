import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidarSeguimientoSemanalComponent } from './form-validar-seguimiento-semanal.component';

describe('FormValidarSeguimientoSemanalComponent', () => {
  let component: FormValidarSeguimientoSemanalComponent;
  let fixture: ComponentFixture<FormValidarSeguimientoSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormValidarSeguimientoSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormValidarSeguimientoSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
