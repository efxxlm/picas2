import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcordionTablaListaProyectosVaotrComponent } from './acordion-tabla-lista-proyectos-vaotr.component';

describe('AcordionTablaListaProyectosVaotrComponent', () => {
  let component: AcordionTablaListaProyectosVaotrComponent;
  let fixture: ComponentFixture<AcordionTablaListaProyectosVaotrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcordionTablaListaProyectosVaotrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcordionTablaListaProyectosVaotrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
