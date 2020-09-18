import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConDisponibilidadPresupuestalComponent } from './tabla-con-disponibilidad-presupuestal.component';

describe('TablaConDisponibilidadPresupuestalComponent', () => {
  let component: TablaConDisponibilidadPresupuestalComponent;
  let fixture: ComponentFixture<TablaConDisponibilidadPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConDisponibilidadPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConDisponibilidadPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
