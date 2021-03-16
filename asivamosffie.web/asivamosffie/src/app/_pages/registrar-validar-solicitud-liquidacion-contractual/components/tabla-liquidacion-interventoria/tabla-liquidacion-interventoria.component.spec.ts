import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaLiquidacionInterventoriaComponent } from './tabla-liquidacion-interventoria.component';

describe('TablaLiquidacionInterventoriaComponent', () => {
  let component: TablaLiquidacionInterventoriaComponent;
  let fixture: ComponentFixture<TablaLiquidacionInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaLiquidacionInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaLiquidacionInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
