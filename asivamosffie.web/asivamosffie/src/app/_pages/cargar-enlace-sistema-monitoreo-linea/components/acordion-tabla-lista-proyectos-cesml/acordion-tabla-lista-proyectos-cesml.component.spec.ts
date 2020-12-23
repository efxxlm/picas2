import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcordionTablaListaProyectosCesmlComponent } from './acordion-tabla-lista-proyectos-cesml.component';

describe('AcordionTablaListaProyectosCesmlComponent', () => {
  let component: AcordionTablaListaProyectosCesmlComponent;
  let fixture: ComponentFixture<AcordionTablaListaProyectosCesmlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcordionTablaListaProyectosCesmlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcordionTablaListaProyectosCesmlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
