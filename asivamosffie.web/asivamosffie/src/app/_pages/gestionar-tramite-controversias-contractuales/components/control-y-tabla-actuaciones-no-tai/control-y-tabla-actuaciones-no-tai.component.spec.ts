import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlYTablaActuacionesNoTaiComponent } from './control-y-tabla-actuaciones-no-tai.component';

describe('ControlYTablaActuacionesNoTaiComponent', () => {
  let component: ControlYTablaActuacionesNoTaiComponent;
  let fixture: ComponentFixture<ControlYTablaActuacionesNoTaiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlYTablaActuacionesNoTaiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlYTablaActuacionesNoTaiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
