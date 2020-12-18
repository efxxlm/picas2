import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AprobarSolicitudesPagoComponent } from './aprobar-solicitudes-pago.component';

describe('AprobarSolicitudesPagoComponent', () => {
  let component: AprobarSolicitudesPagoComponent;
  let fixture: ComponentFixture<AprobarSolicitudesPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AprobarSolicitudesPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AprobarSolicitudesPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
