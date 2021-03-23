import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapaEstadisticasSupervisoresComponent } from './mapa-estadisticas-supervisores.component';

describe('MapaEstadisticasSupervisoresComponent', () => {
  let component: MapaEstadisticasSupervisoresComponent;
  let fixture: ComponentFixture<MapaEstadisticasSupervisoresComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapaEstadisticasSupervisoresComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapaEstadisticasSupervisoresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
