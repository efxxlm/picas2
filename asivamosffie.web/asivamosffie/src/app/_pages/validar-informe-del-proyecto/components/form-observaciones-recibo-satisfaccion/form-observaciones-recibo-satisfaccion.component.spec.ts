import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormObservacionesReciboSatisfaccionComponent } from './form-observaciones-recibo-satisfaccion.component';

describe('FormObservacionesReciboSatisfaccionComponent', () => {
  let component: FormObservacionesReciboSatisfaccionComponent;
  let fixture: ComponentFixture<FormObservacionesReciboSatisfaccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormObservacionesReciboSatisfaccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormObservacionesReciboSatisfaccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
