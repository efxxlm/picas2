import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDecisionesActaComponent } from './tabla-decisiones-acta.component';

describe('TablaDecisionesActaComponent', () => {
  let component: TablaDecisionesActaComponent;
  let fixture: ComponentFixture<TablaDecisionesActaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDecisionesActaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDecisionesActaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
