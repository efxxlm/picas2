import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleActuacionProcesoComponent } from './detalle-actuacion-proceso.component';

describe('DetalleActuacionProcesoComponent', () => {
  let component: DetalleActuacionProcesoComponent;
  let fixture: ComponentFixture<DetalleActuacionProcesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleActuacionProcesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleActuacionProcesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
