import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaHistorialObservacionesComponent } from './tabla-historial-observaciones.component';

describe('TablaHistorialObservacionesComponent', () => {
  let component: TablaHistorialObservacionesComponent;
  let fixture: ComponentFixture<TablaHistorialObservacionesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaHistorialObservacionesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaHistorialObservacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
