import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistroPresupuestalComponent } from './tabla-registro-presupuestal.component';

describe('TablaRegistroPresupuestalComponent', () => {
  let component: TablaRegistroPresupuestalComponent;
  let fixture: ComponentFixture<TablaRegistroPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistroPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistroPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
