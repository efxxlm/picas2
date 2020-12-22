import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaObservacionesAutorizComponent } from './tabla-observaciones-autoriz.component';

describe('TablaObservacionesAutorizComponent', () => {
  let component: TablaObservacionesAutorizComponent;
  let fixture: ComponentFixture<TablaObservacionesAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaObservacionesAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaObservacionesAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
