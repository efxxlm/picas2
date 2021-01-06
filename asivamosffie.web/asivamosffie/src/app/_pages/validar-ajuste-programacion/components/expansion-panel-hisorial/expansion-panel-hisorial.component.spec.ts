import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpansionPanelHisorialComponent } from './expansion-panel-hisorial.component';

describe('ExpansionPanelHisorialComponent', () => {
  let component: ExpansionPanelHisorialComponent;
  let fixture: ComponentFixture<ExpansionPanelHisorialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpansionPanelHisorialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpansionPanelHisorialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
