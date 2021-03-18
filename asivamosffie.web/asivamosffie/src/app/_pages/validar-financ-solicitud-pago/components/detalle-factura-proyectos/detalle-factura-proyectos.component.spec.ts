import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleFacturaProyectosComponent } from './detalle-factura-proyectos.component';

describe('DetalleFacturaProyectosComponent', () => {
  let component: DetalleFacturaProyectosComponent;
  let fixture: ComponentFixture<DetalleFacturaProyectosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleFacturaProyectosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleFacturaProyectosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
