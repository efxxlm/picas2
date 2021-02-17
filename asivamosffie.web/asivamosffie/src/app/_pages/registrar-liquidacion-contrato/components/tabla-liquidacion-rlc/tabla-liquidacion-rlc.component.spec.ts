import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaLiquidacionRlcComponent } from './tabla-liquidacion-rlc.component';

describe('TablaLiquidacionRlcComponent', () => {
  let component: TablaLiquidacionRlcComponent;
  let fixture: ComponentFixture<TablaLiquidacionRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaLiquidacionRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaLiquidacionRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
