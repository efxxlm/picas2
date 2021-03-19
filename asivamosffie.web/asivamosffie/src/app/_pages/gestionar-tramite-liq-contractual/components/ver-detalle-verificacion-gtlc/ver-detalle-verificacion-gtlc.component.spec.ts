import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleVerificacionGtlcComponent } from './ver-detalle-verificacion-gtlc.component';

describe('VerDetalleVerificacionGtlcComponent', () => {
  let component: VerDetalleVerificacionGtlcComponent;
  let fixture: ComponentFixture<VerDetalleVerificacionGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleVerificacionGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleVerificacionGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
