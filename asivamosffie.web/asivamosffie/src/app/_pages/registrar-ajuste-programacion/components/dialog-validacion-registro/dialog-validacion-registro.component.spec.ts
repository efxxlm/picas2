import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogValidacionRegistroComponent } from './dialog-validacion-registro.component';

describe('DialogValidacionRegistroComponent', () => {
  let component: DialogValidacionRegistroComponent;
  let fixture: ComponentFixture<DialogValidacionRegistroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogValidacionRegistroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogValidacionRegistroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
