import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogProyectosAsociadosValidfspComponent } from './dialog-proyectos-asociados-validfsp.component';

describe('DialogProyectosAsociadosValidfspComponent', () => {
  let component: DialogProyectosAsociadosValidfspComponent;
  let fixture: ComponentFixture<DialogProyectosAsociadosValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogProyectosAsociadosValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogProyectosAsociadosValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
