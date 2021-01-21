import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaHistorialObservacionesSolpagoComponent } from './tabla-historial-observaciones-solpago.component';

describe('TablaHistorialObservacionesSolpagoComponent', () => {
  let component: TablaHistorialObservacionesSolpagoComponent;
  let fixture: ComponentFixture<TablaHistorialObservacionesSolpagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaHistorialObservacionesSolpagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaHistorialObservacionesSolpagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
