import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleConValidacionPresupuestalComponent } from './detalle-con-validacion-presupuestal.component';

describe('DetalleConValidacionPresupuestalComponent', () => {
  let component: DetalleConValidacionPresupuestalComponent;
  let fixture: ComponentFixture<DetalleConValidacionPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleConValidacionPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleConValidacionPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
