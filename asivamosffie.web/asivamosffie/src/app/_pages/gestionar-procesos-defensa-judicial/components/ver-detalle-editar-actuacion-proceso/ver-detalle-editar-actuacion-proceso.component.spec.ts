import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleEditarActuacionProcesoComponent } from './ver-detalle-editar-actuacion-proceso.component';

describe('VerDetalleEditarActuacionProcesoComponent', () => {
  let component: VerDetalleEditarActuacionProcesoComponent;
  let fixture: ComponentFixture<VerDetalleEditarActuacionProcesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleEditarActuacionProcesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleEditarActuacionProcesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
