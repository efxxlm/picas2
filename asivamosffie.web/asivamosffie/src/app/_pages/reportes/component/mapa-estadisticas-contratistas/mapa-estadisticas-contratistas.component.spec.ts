import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapaEstadisticasContratistasComponent } from './mapa-estadisticas-contratistas.component';

describe('MapaEstadisticasContratistasComponent', () => {
  let component: MapaEstadisticasContratistasComponent;
  let fixture: ComponentFixture<MapaEstadisticasContratistasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapaEstadisticasContratistasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapaEstadisticasContratistasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
