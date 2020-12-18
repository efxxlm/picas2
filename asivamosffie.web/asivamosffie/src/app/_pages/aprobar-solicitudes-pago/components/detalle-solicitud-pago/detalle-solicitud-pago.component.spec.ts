import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleSolicitudPagoComponent } from './detalle-solicitud-pago.component';

describe('DetalleSolicitudPagoComponent', () => {
  let component: DetalleSolicitudPagoComponent;
  let fixture: ComponentFixture<DetalleSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
