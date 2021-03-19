import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarLiquidacionContratoComponent } from './registrar-liquidacion-contrato.component';

describe('RegistrarLiquidacionContratoComponent', () => {
  let component: RegistrarLiquidacionContratoComponent;
  let fixture: ComponentFixture<RegistrarLiquidacionContratoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarLiquidacionContratoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarLiquidacionContratoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
