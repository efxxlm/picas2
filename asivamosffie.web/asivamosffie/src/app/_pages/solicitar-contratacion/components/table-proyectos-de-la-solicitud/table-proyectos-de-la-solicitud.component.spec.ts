import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableProyectosDeLaSolicitudComponent } from './table-proyectos-de-la-solicitud.component';

describe('TableProyectosDeLaSolicitudComponent', () => {
  let component: TableProyectosDeLaSolicitudComponent;
  let fixture: ComponentFixture<TableProyectosDeLaSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableProyectosDeLaSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableProyectosDeLaSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
