import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleAprobarPreconstruccionComponent } from './ver-detalle-aprobar-preconstruccion.component';

describe('VerDetalleAprobarPreconstruccionComponent', () => {
  let component: VerDetalleAprobarPreconstruccionComponent;
  let fixture: ComponentFixture<VerDetalleAprobarPreconstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleAprobarPreconstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleAprobarPreconstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
