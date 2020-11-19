import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarSeguimientoSemanalComponent } from './form-registrar-seguimiento-semanal.component';

describe('FormRegistrarSeguimientoSemanalComponent', () => {
  let component: FormRegistrarSeguimientoSemanalComponent;
  let fixture: ComponentFixture<FormRegistrarSeguimientoSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarSeguimientoSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarSeguimientoSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
