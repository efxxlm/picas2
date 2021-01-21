import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogObservacionesItemListchequeoComponent } from './dialog-observaciones-item-listchequeo.component';

describe('DialogObservacionesItemListchequeoComponent', () => {
  let component: DialogObservacionesItemListchequeoComponent;
  let fixture: ComponentFixture<DialogObservacionesItemListchequeoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogObservacionesItemListchequeoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogObservacionesItemListchequeoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
