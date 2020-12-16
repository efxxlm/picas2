import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCargarActaSuscritaConstComponent } from './dialog-cargar-acta-suscrita-const.component';

describe('DialogCargarActaSuscritaConstComponent', () => {
  let component: DialogCargarActaSuscritaConstComponent;
  let fixture: ComponentFixture<DialogCargarActaSuscritaConstComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCargarActaSuscritaConstComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCargarActaSuscritaConstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
