import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogObservacionesFlujoRecursosComponent } from './dialog-observaciones-flujo-recursos.component';

describe('DialogObservacionesFlujoRecursosComponent', () => {
  let component: DialogObservacionesFlujoRecursosComponent;
  let fixture: ComponentFixture<DialogObservacionesFlujoRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogObservacionesFlujoRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogObservacionesFlujoRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
