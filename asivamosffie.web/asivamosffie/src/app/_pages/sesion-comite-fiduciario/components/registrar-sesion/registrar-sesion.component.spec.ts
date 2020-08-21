import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarSesionComponent } from './registrar-sesion.component';

describe('RegistrarSesionComponent', () => {
  let component: RegistrarSesionComponent;
  let fixture: ComponentFixture<RegistrarSesionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarSesionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarSesionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
