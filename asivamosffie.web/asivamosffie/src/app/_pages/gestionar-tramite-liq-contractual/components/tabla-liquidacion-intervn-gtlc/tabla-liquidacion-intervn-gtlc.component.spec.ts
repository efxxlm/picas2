import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaLiquidacionIntervnGtlcComponent } from './tabla-liquidacion-intervn-gtlc.component';

describe('TablaLiquidacionIntervnGtlcComponent', () => {
  let component: TablaLiquidacionIntervnGtlcComponent;
  let fixture: ComponentFixture<TablaLiquidacionIntervnGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaLiquidacionIntervnGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaLiquidacionIntervnGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
