import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarRequisitosLiquidacionComponent } from './validar-requisitos-liquidacion.component';

describe('ValidarRequisitosLiquidacionComponent', () => {
  let component: ValidarRequisitosLiquidacionComponent;
  let fixture: ComponentFixture<ValidarRequisitosLiquidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarRequisitosLiquidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarRequisitosLiquidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
