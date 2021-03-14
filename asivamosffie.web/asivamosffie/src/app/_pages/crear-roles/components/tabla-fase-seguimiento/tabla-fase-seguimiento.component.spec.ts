import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFaseSeguimientoComponent } from './tabla-fase-seguimiento.component';

describe('TablaFaseSeguimientoComponent', () => {
  let component: TablaFaseSeguimientoComponent;
  let fixture: ComponentFixture<TablaFaseSeguimientoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFaseSeguimientoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFaseSeguimientoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
