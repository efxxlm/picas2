import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormGestionarUsuariosComponent } from './form-gestionar-usuarios.component';

describe('FormGestionarUsuariosComponent', () => {
  let component: FormGestionarUsuariosComponent;
  let fixture: ComponentFixture<FormGestionarUsuariosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormGestionarUsuariosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormGestionarUsuariosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
