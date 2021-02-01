import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleSolicitudPagoComponent } from './tabla-detalle-solicitud-pago.component';

describe('TablaDetalleSolicitudPagoComponent', () => {
  let component: TablaDetalleSolicitudPagoComponent;
  let fixture: ComponentFixture<TablaDetalleSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
