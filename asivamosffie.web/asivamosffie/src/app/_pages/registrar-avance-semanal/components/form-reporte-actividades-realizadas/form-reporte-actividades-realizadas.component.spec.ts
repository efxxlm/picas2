import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormReporteActividadesRealizadasComponent } from './form-reporte-actividades-realizadas.component';

describe('FormReporteActividadesRealizadasComponent', () => {
  let component: FormReporteActividadesRealizadasComponent;
  let fixture: ComponentFixture<FormReporteActividadesRealizadasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormReporteActividadesRealizadasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormReporteActividadesRealizadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
