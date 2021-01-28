import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarNovedadContratoObraComponent } from './validar-novedad-contrato-obra.component';

describe('ValidarNovedadContratoObraComponent', () => {
  let component: ValidarNovedadContratoObraComponent;
  let fixture: ComponentFixture<ValidarNovedadContratoObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarNovedadContratoObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarNovedadContratoObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
