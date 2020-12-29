import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogObservacionesValidfspComponent } from './dialog-observaciones-validfsp.component';

describe('DialogObservacionesValidfspComponent', () => {
  let component: DialogObservacionesValidfspComponent;
  let fixture: ComponentFixture<DialogObservacionesValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogObservacionesValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogObservacionesValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
