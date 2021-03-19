import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaBalanceFinancieroRlcComponent } from './tabla-balance-financiero-rlc.component';

describe('TablaBalanceFinancieroRlcComponent', () => {
  let component: TablaBalanceFinancieroRlcComponent;
  let fixture: ComponentFixture<TablaBalanceFinancieroRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaBalanceFinancieroRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaBalanceFinancieroRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
