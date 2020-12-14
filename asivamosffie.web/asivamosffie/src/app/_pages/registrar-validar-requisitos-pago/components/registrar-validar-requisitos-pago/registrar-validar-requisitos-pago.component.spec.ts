import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarValidarRequisitosPagoComponent } from './registrar-validar-requisitos-pago.component';

describe('RegistrarValidarRequisitosPagoComponent', () => {
  let component: RegistrarValidarRequisitosPagoComponent;
  let fixture: ComponentFixture<RegistrarValidarRequisitosPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarValidarRequisitosPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarValidarRequisitosPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
