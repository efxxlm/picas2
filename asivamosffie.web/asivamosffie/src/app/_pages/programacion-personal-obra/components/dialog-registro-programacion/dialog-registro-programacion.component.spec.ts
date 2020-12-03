import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogRegistroProgramacionComponent } from './dialog-registro-programacion.component';

describe('DialogRegistroProgramacionComponent', () => {
  let component: DialogRegistroProgramacionComponent;
  let fixture: ComponentFixture<DialogRegistroProgramacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogRegistroProgramacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogRegistroProgramacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
