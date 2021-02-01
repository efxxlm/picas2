import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleFacturaProcasAutorizComponent } from './detalle-factura-procas-autoriz.component';

describe('DetalleFacturaProcasAutorizComponent', () => {
  let component: DetalleFacturaProcasAutorizComponent;
  let fixture: ComponentFixture<DetalleFacturaProcasAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleFacturaProcasAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleFacturaProcasAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
