import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsDatosFacturaComponent } from './obs-datos-factura.component';

describe('ObsDatosFacturaComponent', () => {
  let component: ObsDatosFacturaComponent;
  let fixture: ComponentFixture<ObsDatosFacturaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsDatosFacturaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsDatosFacturaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
