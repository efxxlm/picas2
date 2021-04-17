import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaProyectosRegistrarNovedadComponent } from './tabla-proyectos-registrar-novedad.component';

describe('TablaProyectosRegistrarNovedadComponent', () => {
  let component: TablaProyectosRegistrarNovedadComponent;
  let fixture: ComponentFixture<TablaProyectosRegistrarNovedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaProyectosRegistrarNovedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaProyectosRegistrarNovedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
