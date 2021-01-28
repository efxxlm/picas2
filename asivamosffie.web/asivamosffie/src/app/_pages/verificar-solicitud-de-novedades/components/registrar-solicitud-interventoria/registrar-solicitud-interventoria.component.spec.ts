import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarSolicitudInterventoriaComponent } from './registrar-solicitud-interventoria.component';

describe('RegistrarSolicitudInterventoriaComponent', () => {
  let component: RegistrarSolicitudInterventoriaComponent;
  let fixture: ComponentFixture<RegistrarSolicitudInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarSolicitudInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarSolicitudInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
