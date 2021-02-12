import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTablaObservacionesRapgComponent } from './detalle-tabla-observaciones-rapg.component';

describe('DetalleTablaObservacionesRapgComponent', () => {
  let component: DetalleTablaObservacionesRapgComponent;
  let fixture: ComponentFixture<DetalleTablaObservacionesRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTablaObservacionesRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTablaObservacionesRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
