import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogProyectosAsociadosComponent } from './dialog-proyectos-asociados.component';

describe('DialogProyectosAsociadosComponent', () => {
  let component: DialogProyectosAsociadosComponent;
  let fixture: ComponentFixture<DialogProyectosAsociadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogProyectosAsociadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogProyectosAsociadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
