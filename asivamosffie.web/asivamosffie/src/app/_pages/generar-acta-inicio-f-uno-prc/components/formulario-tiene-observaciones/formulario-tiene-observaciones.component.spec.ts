import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormularioTieneObservacionesComponent } from './formulario-tiene-observaciones.component';

describe('FormularioTieneObservacionesComponent', () => {
  let component: FormularioTieneObservacionesComponent;
  let fixture: ComponentFixture<FormularioTieneObservacionesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormularioTieneObservacionesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormularioTieneObservacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
