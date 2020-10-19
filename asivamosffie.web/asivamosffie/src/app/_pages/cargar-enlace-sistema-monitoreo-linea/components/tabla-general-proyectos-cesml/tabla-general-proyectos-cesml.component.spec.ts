import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGeneralProyectosCesmlComponent } from './tabla-general-proyectos-cesml.component';

describe('TablaGeneralProyectosCesmlComponent', () => {
  let component: TablaGeneralProyectosCesmlComponent;
  let fixture: ComponentFixture<TablaGeneralProyectosCesmlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGeneralProyectosCesmlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGeneralProyectosCesmlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
