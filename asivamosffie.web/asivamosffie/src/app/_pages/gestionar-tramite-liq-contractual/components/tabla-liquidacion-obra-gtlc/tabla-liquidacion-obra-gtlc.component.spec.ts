import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaLiquidacionObraGtlcComponent } from './tabla-liquidacion-obra-gtlc.component';

describe('TablaLiquidacionObraGtlcComponent', () => {
  let component: TablaLiquidacionObraGtlcComponent;
  let fixture: ComponentFixture<TablaLiquidacionObraGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaLiquidacionObraGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaLiquidacionObraGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
