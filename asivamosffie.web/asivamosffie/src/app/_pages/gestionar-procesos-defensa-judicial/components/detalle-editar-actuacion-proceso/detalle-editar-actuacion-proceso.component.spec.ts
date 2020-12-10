import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleEditarActuacionProcesoComponent } from './detalle-editar-actuacion-proceso.component';

describe('DetalleEditarActuacionProcesoComponent', () => {
  let component: DetalleEditarActuacionProcesoComponent;
  let fixture: ComponentFixture<DetalleEditarActuacionProcesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleEditarActuacionProcesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleEditarActuacionProcesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
