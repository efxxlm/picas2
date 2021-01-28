import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleAutorizarSolicitudComponent } from './ver-detalle-autorizar-solicitud.component';

describe('VerDetalleAutorizarSolicitudComponent', () => {
  let component: VerDetalleAutorizarSolicitudComponent;
  let fixture: ComponentFixture<VerDetalleAutorizarSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleAutorizarSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleAutorizarSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
