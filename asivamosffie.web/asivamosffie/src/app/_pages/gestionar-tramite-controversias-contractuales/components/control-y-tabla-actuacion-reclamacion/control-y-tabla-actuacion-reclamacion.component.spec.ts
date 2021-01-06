import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlYTablaActuacionReclamacionComponent } from './control-y-tabla-actuacion-reclamacion.component';

describe('ControlYTablaActuacionReclamacionComponent', () => {
  let component: ControlYTablaActuacionReclamacionComponent;
  let fixture: ComponentFixture<ControlYTablaActuacionReclamacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlYTablaActuacionReclamacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlYTablaActuacionReclamacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
