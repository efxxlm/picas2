import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleDisponibilidadPresupuestalComponent } from './detalle-disponibilidad-presupuestal.component';

describe('DetalleDisponibilidadPresupuestalComponent', () => {
  let component: DetalleDisponibilidadPresupuestalComponent;
  let fixture: ComponentFixture<DetalleDisponibilidadPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleDisponibilidadPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleDisponibilidadPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
