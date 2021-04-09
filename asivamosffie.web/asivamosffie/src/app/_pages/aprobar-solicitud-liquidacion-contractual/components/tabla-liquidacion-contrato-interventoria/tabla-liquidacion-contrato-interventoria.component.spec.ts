import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaLiquidacionContratoInterventoriaComponent } from './tabla-liquidacion-contrato-interventoria.component';

describe('TablaLiquidacionContratoInterventoriaComponent', () => {
  let component: TablaLiquidacionContratoInterventoriaComponent;
  let fixture: ComponentFixture<TablaLiquidacionContratoInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaLiquidacionContratoInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaLiquidacionContratoInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
