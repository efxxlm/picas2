import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogTableProyectosSeleccionadosComponent } from './dialog-table-proyectos-seleccionados.component';

describe('DialogTableProyectosSeleccionadosComponent', () => {
  let component: DialogTableProyectosSeleccionadosComponent;
  let fixture: ComponentFixture<DialogTableProyectosSeleccionadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogTableProyectosSeleccionadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogTableProyectosSeleccionadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
