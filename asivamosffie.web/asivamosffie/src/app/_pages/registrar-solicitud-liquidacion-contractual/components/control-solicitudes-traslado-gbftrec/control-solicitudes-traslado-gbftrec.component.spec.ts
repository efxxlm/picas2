import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlSolicitudesTrasladoGbftrecComponent } from './control-solicitudes-traslado-gbftrec.component';

describe('ControlSolicitudesTrasladoGbftrecComponent', () => {
  let component: ControlSolicitudesTrasladoGbftrecComponent;
  let fixture: ComponentFixture<ControlSolicitudesTrasladoGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlSolicitudesTrasladoGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlSolicitudesTrasladoGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
