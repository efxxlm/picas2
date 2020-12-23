import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargarEnlaceSistemaMonitoreoLineaComponent } from './cargar-enlace-sistema-monitoreo-linea.component';

describe('CargarEnlaceSistemaMonitoreoLineaComponent', () => {
  let component: CargarEnlaceSistemaMonitoreoLineaComponent;
  let fixture: ComponentFixture<CargarEnlaceSistemaMonitoreoLineaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargarEnlaceSistemaMonitoreoLineaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargarEnlaceSistemaMonitoreoLineaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
