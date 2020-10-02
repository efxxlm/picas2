import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaProyectosAsociadosComponent } from './tabla-proyectos-asociados.component';

describe('TablaProyectosAsociadosComponent', () => {
  let component: TablaProyectosAsociadosComponent;
  let fixture: ComponentFixture<TablaProyectosAsociadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaProyectosAsociadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaProyectosAsociadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
