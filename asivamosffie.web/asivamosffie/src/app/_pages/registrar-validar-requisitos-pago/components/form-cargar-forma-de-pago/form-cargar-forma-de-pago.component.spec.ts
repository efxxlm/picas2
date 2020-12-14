import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCargarFormaDePagoComponent } from './form-cargar-forma-de-pago.component';

describe('FormCargarFormaDePagoComponent', () => {
  let component: FormCargarFormaDePagoComponent;
  let fixture: ComponentFixture<FormCargarFormaDePagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCargarFormaDePagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCargarFormaDePagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
