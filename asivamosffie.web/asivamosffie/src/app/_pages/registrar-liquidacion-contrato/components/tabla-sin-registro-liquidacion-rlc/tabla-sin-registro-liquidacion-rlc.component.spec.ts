import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSinRegistroLiquidacionRlcComponent } from './tabla-sin-registro-liquidacion-rlc.component';

describe('TablaSinRegistroLiquidacionRlcComponent', () => {
  let component: TablaSinRegistroLiquidacionRlcComponent;
  let fixture: ComponentFixture<TablaSinRegistroLiquidacionRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSinRegistroLiquidacionRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSinRegistroLiquidacionRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
