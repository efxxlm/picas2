import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaHistorialObservacionesPolizaComponent } from './tabla-historial-observaciones-poliza.component';

describe('TablaHistorialObservacionesPolizaComponent', () => {
  let component: TablaHistorialObservacionesPolizaComponent;
  let fixture: ComponentFixture<TablaHistorialObservacionesPolizaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaHistorialObservacionesPolizaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaHistorialObservacionesPolizaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
