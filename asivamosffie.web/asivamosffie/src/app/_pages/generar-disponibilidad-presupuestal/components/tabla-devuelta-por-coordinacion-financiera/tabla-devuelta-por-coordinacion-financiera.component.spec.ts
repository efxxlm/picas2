import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDevueltaPorCoordinacionFinancieraComponent } from './tabla-devuelta-por-coordinacion-financiera.component';

describe('TablaDevueltaPorCoordinacionFinancieraComponent', () => {
  let component: TablaDevueltaPorCoordinacionFinancieraComponent;
  let fixture: ComponentFixture<TablaDevueltaPorCoordinacionFinancieraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDevueltaPorCoordinacionFinancieraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDevueltaPorCoordinacionFinancieraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
