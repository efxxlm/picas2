import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarNovedadContratoInterventoriaComponent } from './validar-novedad-contrato-interventoria.component';

describe('ValidarNovedadContratoInterventoriaComponent', () => {
  let component: ValidarNovedadContratoInterventoriaComponent;
  let fixture: ComponentFixture<ValidarNovedadContratoInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarNovedadContratoInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarNovedadContratoInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
