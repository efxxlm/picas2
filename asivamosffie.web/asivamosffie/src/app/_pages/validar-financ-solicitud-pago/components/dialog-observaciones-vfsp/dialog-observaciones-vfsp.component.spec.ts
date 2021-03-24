import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogObservacionesVfspComponent } from './dialog-observaciones-vfsp.component';

describe('DialogObservacionesVfspComponent', () => {
  let component: DialogObservacionesVfspComponent;
  let fixture: ComponentFixture<DialogObservacionesVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogObservacionesVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogObservacionesVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
