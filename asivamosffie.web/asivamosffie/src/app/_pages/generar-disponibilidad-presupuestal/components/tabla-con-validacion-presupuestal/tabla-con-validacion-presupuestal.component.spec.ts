import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConValidacionPresupuestalComponent } from './tabla-con-validacion-presupuestal.component';

describe('TablaConValidacionPresupuestalComponent', () => {
  let component: TablaConValidacionPresupuestalComponent;
  let fixture: ComponentFixture<TablaConValidacionPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConValidacionPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConValidacionPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
