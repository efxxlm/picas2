import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarPagosRendimientosComponent } from './registrar-pagos-rendimientos.component';

describe('RegistrarPagosRendimientosComponent', () => {
  let component: RegistrarPagosRendimientosComponent;
  let fixture: ComponentFixture<RegistrarPagosRendimientosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarPagosRendimientosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarPagosRendimientosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
