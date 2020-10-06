import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDevueltaPorCoordinacionComponent } from './tabla-devuelta-por-coordinacion.component';

describe('TablaDevueltaPorCoordinacionComponent', () => {
  let component: TablaDevueltaPorCoordinacionComponent;
  let fixture: ComponentFixture<TablaDevueltaPorCoordinacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDevueltaPorCoordinacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDevueltaPorCoordinacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
