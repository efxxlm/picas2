import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormGenerarActaInicioConstTecnicoComponent } from './form-generar-acta-inicio-const-tecnico.component';

describe('FormGenerarActaInicioConstTecnicoComponent', () => {
  let component: FormGenerarActaInicioConstTecnicoComponent;
  let fixture: ComponentFixture<FormGenerarActaInicioConstTecnicoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormGenerarActaInicioConstTecnicoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormGenerarActaInicioConstTecnicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
