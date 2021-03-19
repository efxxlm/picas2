import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleActuPolizaGtlcComponent } from './detalle-actu-poliza-gtlc.component';

describe('DetalleActuPolizaGtlcComponent', () => {
  let component: DetalleActuPolizaGtlcComponent;
  let fixture: ComponentFixture<DetalleActuPolizaGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleActuPolizaGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleActuPolizaGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
