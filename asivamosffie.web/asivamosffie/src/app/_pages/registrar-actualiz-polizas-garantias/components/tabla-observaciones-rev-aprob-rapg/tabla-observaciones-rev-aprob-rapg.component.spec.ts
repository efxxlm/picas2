import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaObservacionesRevAprobRapgComponent } from './tabla-observaciones-rev-aprob-rapg.component';

describe('TablaObservacionesRevAprobRapgComponent', () => {
  let component: TablaObservacionesRevAprobRapgComponent;
  let fixture: ComponentFixture<TablaObservacionesRevAprobRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaObservacionesRevAprobRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaObservacionesRevAprobRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
