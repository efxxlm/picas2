import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCargarActaComponent } from './dialog-cargar-acta.component';

describe('DialogCargarActaComponent', () => {
  let component: DialogCargarActaComponent;
  let fixture: ComponentFixture<DialogCargarActaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCargarActaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCargarActaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
