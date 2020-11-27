import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaAvanceFinancieroComponent } from './tabla-avance-financiero.component';

describe('TablaAvanceFinancieroComponent', () => {
  let component: TablaAvanceFinancieroComponent;
  let fixture: ComponentFixture<TablaAvanceFinancieroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaAvanceFinancieroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaAvanceFinancieroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
