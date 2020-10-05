import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDevueltaPorValidacionComponent } from './tabla-devuelta-por-validacion.component';

describe('TablaDevueltaPorValidacionComponent', () => {
  let component: TablaDevueltaPorValidacionComponent;
  let fixture: ComponentFixture<TablaDevueltaPorValidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDevueltaPorValidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDevueltaPorValidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
