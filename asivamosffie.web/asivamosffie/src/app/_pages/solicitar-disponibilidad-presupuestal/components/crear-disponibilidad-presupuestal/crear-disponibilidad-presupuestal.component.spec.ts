import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrearSolicitudDeDisponibilidadPresupuestalComponent } from './crear-solicitud-de-disponibilidad-presupuestal.component';

describe('CrearSolicitudDeDisponibilidadPresupuestalComponent', () => {
  let component: CrearSolicitudDeDisponibilidadPresupuestalComponent;
  let fixture: ComponentFixture<CrearSolicitudDeDisponibilidadPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrearSolicitudDeDisponibilidadPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrearSolicitudDeDisponibilidadPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
