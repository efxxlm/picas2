import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleEditarRegistroProcesoDjComponent } from './ver-detalle-editar-registro-proceso-dj.component';

describe('VerDetalleEditarRegistroProcesoDjComponent', () => {
  let component: VerDetalleEditarRegistroProcesoDjComponent;
  let fixture: ComponentFixture<VerDetalleEditarRegistroProcesoDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleEditarRegistroProcesoDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleEditarRegistroProcesoDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
