import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogObservacionesProgramacionComponent } from './dialog-observaciones-programacion.component';

describe('DialogObservacionesProgramacionComponent', () => {
  let component: DialogObservacionesProgramacionComponent;
  let fixture: ComponentFixture<DialogObservacionesProgramacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogObservacionesProgramacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogObservacionesProgramacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
