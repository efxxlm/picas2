import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormVerificarSeguimientoSemanalComponent } from './form-verificar-seguimiento-semanal.component';

describe('FormVerificarSeguimientoSemanalComponent', () => {
  let component: FormVerificarSeguimientoSemanalComponent;
  let fixture: ComponentFixture<FormVerificarSeguimientoSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormVerificarSeguimientoSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormVerificarSeguimientoSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
