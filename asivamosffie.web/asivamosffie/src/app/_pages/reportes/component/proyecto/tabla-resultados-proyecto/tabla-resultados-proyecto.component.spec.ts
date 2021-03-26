import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaResultadosProyectoComponent } from './tabla-resultados-proyecto.component';

describe('TablaResultadosProyectoComponent', () => {
  let component: TablaResultadosProyectoComponent;
  let fixture: ComponentFixture<TablaResultadosProyectoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaResultadosProyectoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaResultadosProyectoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
