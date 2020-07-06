import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaProyectosComponent } from './tabla-proyectos.component';

describe('TablaProyectosComponent', () => {
  let component: TablaProyectosComponent;
  let fixture: ComponentFixture<TablaProyectosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaProyectosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaProyectosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
