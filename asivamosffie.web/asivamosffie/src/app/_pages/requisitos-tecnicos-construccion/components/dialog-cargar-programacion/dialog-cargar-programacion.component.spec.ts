import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCargarProgramacionComponent } from './dialog-cargar-programacion.component';

describe('DialogCargarProgramacionComponent', () => {
  let component: DialogCargarProgramacionComponent;
  let fixture: ComponentFixture<DialogCargarProgramacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCargarProgramacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCargarProgramacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
