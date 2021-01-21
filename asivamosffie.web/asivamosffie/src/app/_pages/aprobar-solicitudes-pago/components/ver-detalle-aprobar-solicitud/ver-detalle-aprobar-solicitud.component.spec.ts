import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleAprobarSolicitudComponent } from './ver-detalle-aprobar-solicitud.component';

describe('VerDetalleAprobarSolicitudComponent', () => {
  let component: VerDetalleAprobarSolicitudComponent;
  let fixture: ComponentFixture<VerDetalleAprobarSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleAprobarSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleAprobarSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
