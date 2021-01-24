import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlTablaActuacionProcesoComponent } from './control-tabla-actuacion-proceso.component';

describe('ControlTablaActuacionProcesoComponent', () => {
  let component: ControlTablaActuacionProcesoComponent;
  let fixture: ComponentFixture<ControlTablaActuacionProcesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlTablaActuacionProcesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlTablaActuacionProcesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
