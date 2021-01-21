import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleFacturaProyectosAsociadosComponent } from './detalle-factura-proyectos-asociados.component';

describe('DetalleFacturaProyectosAsociadosComponent', () => {
  let component: DetalleFacturaProyectosAsociadosComponent;
  let fixture: ComponentFixture<DetalleFacturaProyectosAsociadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleFacturaProyectosAsociadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleFacturaProyectosAsociadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
