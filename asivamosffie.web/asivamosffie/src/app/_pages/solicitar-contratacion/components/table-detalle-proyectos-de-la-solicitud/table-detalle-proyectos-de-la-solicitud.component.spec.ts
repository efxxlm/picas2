import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableDetalleProyectosDeLaSolicitudComponent } from './table-detalle-proyectos-de-la-solicitud.component';

describe('TableDetalleProyectosDeLaSolicitudComponent', () => {
  let component: TableDetalleProyectosDeLaSolicitudComponent;
  let fixture: ComponentFixture<TableDetalleProyectosDeLaSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableDetalleProyectosDeLaSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableDetalleProyectosDeLaSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
