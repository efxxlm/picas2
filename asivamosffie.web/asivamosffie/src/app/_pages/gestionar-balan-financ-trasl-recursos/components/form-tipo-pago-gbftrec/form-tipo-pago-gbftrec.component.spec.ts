import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormTipoPagoGbftrecComponent } from './form-tipo-pago-gbftrec.component';

describe('FormTipoPagoGbftrecComponent', () => {
  let component: FormTipoPagoGbftrecComponent;
  let fixture: ComponentFixture<FormTipoPagoGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormTipoPagoGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormTipoPagoGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
