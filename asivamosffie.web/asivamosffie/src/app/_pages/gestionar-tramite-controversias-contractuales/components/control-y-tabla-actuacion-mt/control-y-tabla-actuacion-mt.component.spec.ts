import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlYTablaActuacionMtComponent } from './control-y-tabla-actuacion-mt.component';

describe('ControlYTablaActuacionMtComponent', () => {
  let component: ControlYTablaActuacionMtComponent;
  let fixture: ComponentFixture<ControlYTablaActuacionMtComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlYTablaActuacionMtComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlYTablaActuacionMtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
