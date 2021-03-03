import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleReciboSatisfaccionComponent } from './detalle-recibo-satisfaccion.component';

describe('DetalleReciboSatisfaccionComponent', () => {
  let component: DetalleReciboSatisfaccionComponent;
  let fixture: ComponentFixture<DetalleReciboSatisfaccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleReciboSatisfaccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleReciboSatisfaccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
