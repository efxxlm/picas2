import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarNuevaActuacionTramiteComponent } from './registrar-nueva-actuacion-tramite.component';

describe('RegistrarNuevaActuacionTramiteComponent', () => {
  let component: RegistrarNuevaActuacionTramiteComponent;
  let fixture: ComponentFixture<RegistrarNuevaActuacionTramiteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarNuevaActuacionTramiteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarNuevaActuacionTramiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
