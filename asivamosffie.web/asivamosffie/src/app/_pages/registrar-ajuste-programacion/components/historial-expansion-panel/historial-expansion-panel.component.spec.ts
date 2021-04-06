import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HistorialExpansionPanelComponent } from './historial-expansion-panel.component';

describe('HistorialExpansionPanelComponent', () => {
  let component: HistorialExpansionPanelComponent;
  let fixture: ComponentFixture<HistorialExpansionPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HistorialExpansionPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HistorialExpansionPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
