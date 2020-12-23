import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpansionPanelInterventoriaComponent } from './expansion-panel-interventoria.component';

describe('ExpansionPanelInterventoriaComponent', () => {
  let component: ExpansionPanelInterventoriaComponent;
  let fixture: ComponentFixture<ExpansionPanelInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpansionPanelInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpansionPanelInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
