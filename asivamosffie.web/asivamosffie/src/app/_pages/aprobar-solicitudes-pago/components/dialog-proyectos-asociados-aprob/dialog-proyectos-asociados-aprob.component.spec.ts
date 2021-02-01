import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogProyectosAsociadosAprobComponent } from './dialog-proyectos-asociados-aprob.component';

describe('DialogProyectosAsociadosAprobComponent', () => {
  let component: DialogProyectosAsociadosAprobComponent;
  let fixture: ComponentFixture<DialogProyectosAsociadosAprobComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogProyectosAsociadosAprobComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogProyectosAsociadosAprobComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
