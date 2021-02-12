import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaObservacionesComponent } from './tabla-observaciones.component';

describe('TablaObservacionesComponent', () => {
  let component: TablaObservacionesComponent;
  let fixture: ComponentFixture<TablaObservacionesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaObservacionesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaObservacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
