import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleRegistroPresupuestalComponent } from './ver-detalle-registro-presupuestal.component';

describe('VerDetalleRegistroPresupuestalComponent', () => {
  let component: VerDetalleRegistroPresupuestalComponent;
  let fixture: ComponentFixture<VerDetalleRegistroPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleRegistroPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleRegistroPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
