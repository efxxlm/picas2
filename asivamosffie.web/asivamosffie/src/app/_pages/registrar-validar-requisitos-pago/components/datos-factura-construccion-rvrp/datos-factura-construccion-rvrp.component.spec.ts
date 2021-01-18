import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosFacturaConstruccionRvrpComponent } from './datos-factura-construccion-rvrp.component';

describe('DatosFacturaConstruccionRvrpComponent', () => {
  let component: DatosFacturaConstruccionRvrpComponent;
  let fixture: ComponentFixture<DatosFacturaConstruccionRvrpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatosFacturaConstruccionRvrpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatosFacturaConstruccionRvrpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
