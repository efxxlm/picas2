import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaLiquidacionContratoObraComponent } from './tabla-liquidacion-contrato-obra.component';

describe('TablaLiquidacionContratoObraComponent', () => {
  let component: TablaLiquidacionContratoObraComponent;
  let fixture: ComponentFixture<TablaLiquidacionContratoObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaLiquidacionContratoObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaLiquidacionContratoObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
