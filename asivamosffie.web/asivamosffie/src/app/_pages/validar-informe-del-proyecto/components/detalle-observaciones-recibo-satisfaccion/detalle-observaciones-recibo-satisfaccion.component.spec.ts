import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleObservacionesReciboSatisfaccionComponent } from './detalle-observaciones-recibo-satisfaccion.component';

describe('DetalleObservacionesReciboSatisfaccionComponent', () => {
  let component: DetalleObservacionesReciboSatisfaccionComponent;
  let fixture: ComponentFixture<DetalleObservacionesReciboSatisfaccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleObservacionesReciboSatisfaccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleObservacionesReciboSatisfaccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
