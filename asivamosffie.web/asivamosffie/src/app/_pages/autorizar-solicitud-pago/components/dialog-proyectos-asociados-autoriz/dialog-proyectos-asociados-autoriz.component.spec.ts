import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogProyectosAsociadosAutorizComponent } from './dialog-proyectos-asociados-autoriz.component';

describe('DialogProyectosAsociadosAutorizComponent', () => {
  let component: DialogProyectosAsociadosAutorizComponent;
  let fixture: ComponentFixture<DialogProyectosAsociadosAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogProyectosAsociadosAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogProyectosAsociadosAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
