import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDetalleFacturaProyectosComponent } from './form-detalle-factura-proyectos.component';

describe('FormDetalleFacturaProyectosComponent', () => {
  let component: FormDetalleFacturaProyectosComponent;
  let fixture: ComponentFixture<FormDetalleFacturaProyectosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDetalleFacturaProyectosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDetalleFacturaProyectosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
