import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarSolicitudRlcComponent } from './gestionar-solicitud-rlc.component';

describe('GestionarSolicitudRlcComponent', () => {
  let component: GestionarSolicitudRlcComponent;
  let fixture: ComponentFixture<GestionarSolicitudRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarSolicitudRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarSolicitudRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
