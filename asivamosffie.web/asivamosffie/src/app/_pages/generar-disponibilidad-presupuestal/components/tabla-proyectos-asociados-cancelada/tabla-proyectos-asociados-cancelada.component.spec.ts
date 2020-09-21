import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaProyectosAsociadosCanceladaComponent } from './tabla-proyectos-asociados-cancelada.component';

describe('TablaProyectosAsociadosCanceladaComponent', () => {
  let component: TablaProyectosAsociadosCanceladaComponent;
  let fixture: ComponentFixture<TablaProyectosAsociadosCanceladaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaProyectosAsociadosCanceladaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaProyectosAsociadosCanceladaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
