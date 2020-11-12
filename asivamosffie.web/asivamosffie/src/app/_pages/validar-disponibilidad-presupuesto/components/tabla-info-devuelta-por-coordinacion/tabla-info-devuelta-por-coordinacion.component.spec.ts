import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInfoDevueltaPorCoordinacionComponent } from './tabla-info-devuelta-por-coordinacion.component';

describe('TablaInfoDevueltaPorCoordinacionComponent', () => {
  let component: TablaInfoDevueltaPorCoordinacionComponent;
  let fixture: ComponentFixture<TablaInfoDevueltaPorCoordinacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInfoDevueltaPorCoordinacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInfoDevueltaPorCoordinacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
