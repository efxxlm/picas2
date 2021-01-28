import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFlujoIntervencionRecursosComponent } from './tabla-flujo-intervencion-recursos.component';

describe('TablaFlujoIntervencionRecursosComponent', () => {
  let component: TablaFlujoIntervencionRecursosComponent;
  let fixture: ComponentFixture<TablaFlujoIntervencionRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFlujoIntervencionRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFlujoIntervencionRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
