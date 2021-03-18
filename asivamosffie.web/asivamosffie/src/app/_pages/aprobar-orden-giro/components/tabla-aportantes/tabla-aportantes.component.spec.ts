import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaAportantesComponent } from './tabla-aportantes.component';

describe('TablaAportantesComponent', () => {
  let component: TablaAportantesComponent;
  let fixture: ComponentFixture<TablaAportantesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaAportantesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaAportantesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
