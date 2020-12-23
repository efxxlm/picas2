import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarSolicitudNovedadComponent } from './verificar-solicitud-novedad.component';

describe('VerificarSolicitudNovedadComponent', () => {
  let component: VerificarSolicitudNovedadComponent;
  let fixture: ComponentFixture<VerificarSolicitudNovedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarSolicitudNovedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarSolicitudNovedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
