import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutorizarSolicitudPagoComponent } from './autorizar-solicitud-pago.component';

describe('AutorizarSolicitudPagoComponent', () => {
  let component: AutorizarSolicitudPagoComponent;
  let fixture: ComponentFixture<AutorizarSolicitudPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutorizarSolicitudPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutorizarSolicitudPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
