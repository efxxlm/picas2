import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRecursosAportantesComponent } from './tabla-recursos-aportantes.component';

describe('TablaRecursosAportantesComponent', () => {
  let component: TablaRecursosAportantesComponent;
  let fixture: ComponentFixture<TablaRecursosAportantesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRecursosAportantesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRecursosAportantesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
