import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargarListadoDeProyectosComponent } from './cargar-listado-de-proyectos.component';

describe('CargarListadoDeProyectosComponent', () => {
  let component: CargarListadoDeProyectosComponent;
  let fixture: ComponentFixture<CargarListadoDeProyectosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargarListadoDeProyectosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargarListadoDeProyectosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
