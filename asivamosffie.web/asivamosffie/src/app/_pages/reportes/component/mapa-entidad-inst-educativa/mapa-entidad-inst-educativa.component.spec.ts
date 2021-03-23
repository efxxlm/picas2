import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapaEntidadInstEducativaComponent } from './mapa-entidad-inst-educativa.component';

describe('MapaEntidadInstEducativaComponent', () => {
  let component: MapaEntidadInstEducativaComponent;
  let fixture: ComponentFixture<MapaEntidadInstEducativaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapaEntidadInstEducativaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapaEntidadInstEducativaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
