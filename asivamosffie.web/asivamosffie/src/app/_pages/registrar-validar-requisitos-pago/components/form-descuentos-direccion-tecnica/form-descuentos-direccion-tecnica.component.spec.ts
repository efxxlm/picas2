import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDescuentosDireccionTecnicaComponent } from './form-descuentos-direccion-tecnica.component';

describe('FormDescuentosDireccionTecnicaComponent', () => {
  let component: FormDescuentosDireccionTecnicaComponent;
  let fixture: ComponentFixture<FormDescuentosDireccionTecnicaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDescuentosDireccionTecnicaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDescuentosDireccionTecnicaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
