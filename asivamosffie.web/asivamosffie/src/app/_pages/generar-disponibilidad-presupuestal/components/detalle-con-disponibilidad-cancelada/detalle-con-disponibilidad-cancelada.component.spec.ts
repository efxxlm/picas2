import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleConDisponibilidadCanceladaComponent } from './detalle-con-disponibilidad-cancelada.component';

describe('DetalleConDisponibilidadCanceladaComponent', () => {
  let component: DetalleConDisponibilidadCanceladaComponent;
  let fixture: ComponentFixture<DetalleConDisponibilidadCanceladaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleConDisponibilidadCanceladaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleConDisponibilidadCanceladaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
