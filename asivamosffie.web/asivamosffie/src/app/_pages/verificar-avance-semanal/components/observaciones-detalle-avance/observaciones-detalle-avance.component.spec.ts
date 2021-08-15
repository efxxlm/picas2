import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesDetalleAvanceComponent } from './observaciones-detalle-avance.component';

describe('ObservacionesDetalleAvanceComponent', () => {
  let component: ObservacionesDetalleAvanceComponent;
  let fixture: ComponentFixture<ObservacionesDetalleAvanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesDetalleAvanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesDetalleAvanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
