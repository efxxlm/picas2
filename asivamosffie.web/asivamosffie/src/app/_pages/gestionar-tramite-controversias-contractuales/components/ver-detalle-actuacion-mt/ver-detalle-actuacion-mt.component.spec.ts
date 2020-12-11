import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleActuacionMtComponent } from './ver-detalle-actuacion-mt.component';

describe('VerDetalleActuacionMtComponent', () => {
  let component: VerDetalleActuacionMtComponent;
  let fixture: ComponentFixture<VerDetalleActuacionMtComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleActuacionMtComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleActuacionMtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
