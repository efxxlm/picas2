import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargarMasivamenteProyectosViabilizadosComponent } from './cargar-masivamente-proyectos-viabilizados.component';

describe('CargarMasivamenteProyectosViabilizadosComponent', () => {
  let component: CargarMasivamenteProyectosViabilizadosComponent;
  let fixture: ComponentFixture<CargarMasivamenteProyectosViabilizadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargarMasivamenteProyectosViabilizadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargarMasivamenteProyectosViabilizadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
