import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaObservacionesReciboSatisfaccionComponent } from './tabla-observaciones-recibo-satisfaccion.component';

describe('TablaObservacionesReciboSatisfaccionComponent', () => {
  let component: TablaObservacionesReciboSatisfaccionComponent;
  let fixture: ComponentFixture<TablaObservacionesReciboSatisfaccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaObservacionesReciboSatisfaccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaObservacionesReciboSatisfaccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
