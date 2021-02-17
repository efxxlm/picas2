import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleEditGestionSolicitudRlcComponent } from './verdetalle-edit-gestion-solicitud-rlc.component';

describe('VerdetalleEditGestionSolicitudRlcComponent', () => {
  let component: VerdetalleEditGestionSolicitudRlcComponent;
  let fixture: ComponentFixture<VerdetalleEditGestionSolicitudRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleEditGestionSolicitudRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleEditGestionSolicitudRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
