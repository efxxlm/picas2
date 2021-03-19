import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaBalanceFinancieroGtlcComponent } from './tabla-balance-financiero-gtlc.component';

describe('TablaBalanceFinancieroGtlcComponent', () => {
  let component: TablaBalanceFinancieroGtlcComponent;
  let fixture: ComponentFixture<TablaBalanceFinancieroGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaBalanceFinancieroGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaBalanceFinancieroGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
