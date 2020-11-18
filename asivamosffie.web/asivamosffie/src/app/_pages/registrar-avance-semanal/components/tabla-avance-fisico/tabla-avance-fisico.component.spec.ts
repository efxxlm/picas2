import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaAvanceFisicoComponent } from './tabla-avance-fisico.component';

describe('TablaAvanceFisicoComponent', () => {
  let component: TablaAvanceFisicoComponent;
  let fixture: ComponentFixture<TablaAvanceFisicoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaAvanceFisicoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaAvanceFisicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
