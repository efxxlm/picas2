import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarSesionComiteTecnicoComponent } from './registrar-sesion-comite-tecnico.component';

describe('RegistrarSesionComiteTecnicoComponent', () => {
  let component: RegistrarSesionComiteTecnicoComponent;
  let fixture: ComponentFixture<RegistrarSesionComiteTecnicoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarSesionComiteTecnicoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarSesionComiteTecnicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
