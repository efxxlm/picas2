import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsDatosFacturaAutorizComponent } from './obs-datos-factura-autoriz.component';

describe('ObsDatosFacturaAutorizComponent', () => {
  let component: ObsDatosFacturaAutorizComponent;
  let fixture: ComponentFixture<ObsDatosFacturaAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsDatosFacturaAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsDatosFacturaAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
