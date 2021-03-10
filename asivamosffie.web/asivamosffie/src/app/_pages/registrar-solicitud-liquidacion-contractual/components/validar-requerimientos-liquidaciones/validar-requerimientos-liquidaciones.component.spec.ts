import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarRequerimientosLiquidacionesComponent } from './validar-requerimientos-liquidaciones.component';

describe('ValidarRequerimientosLiquidacionesComponent', () => {
  let component: ValidarRequerimientosLiquidacionesComponent;
  let fixture: ComponentFixture<ValidarRequerimientosLiquidacionesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarRequerimientosLiquidacionesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarRequerimientosLiquidacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
