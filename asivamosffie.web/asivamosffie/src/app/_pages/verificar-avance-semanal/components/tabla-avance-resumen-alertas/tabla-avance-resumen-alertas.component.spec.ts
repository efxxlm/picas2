import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaAvanceResumenAlertasComponent } from './tabla-avance-resumen-alertas.component';

describe('TablaAvanceResumenAlertasComponent', () => {
  let component: TablaAvanceResumenAlertasComponent;
  let fixture: ComponentFixture<TablaAvanceResumenAlertasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaAvanceResumenAlertasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaAvanceResumenAlertasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
