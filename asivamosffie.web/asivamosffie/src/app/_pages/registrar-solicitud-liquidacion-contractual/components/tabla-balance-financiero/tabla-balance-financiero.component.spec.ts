import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaBalanceFinancieroComponent } from './tabla-balance-financiero.component';

describe('TablaBalanceFinancieroComponent', () => {
  let component: TablaBalanceFinancieroComponent;
  let fixture: ComponentFixture<TablaBalanceFinancieroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaBalanceFinancieroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaBalanceFinancieroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
