import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFaseCierreComponent } from './tabla-fase-cierre.component';

describe('TablaFaseCierreComponent', () => {
  let component: TablaFaseCierreComponent;
  let fixture: ComponentFixture<TablaFaseCierreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFaseCierreComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFaseCierreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
