import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaLiquidacionObraComponent } from './tabla-liquidacion-obra.component';

describe('TablaLiquidacionObraComponent', () => {
  let component: TablaLiquidacionObraComponent;
  let fixture: ComponentFixture<TablaLiquidacionObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaLiquidacionObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaLiquidacionObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
