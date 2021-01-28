import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsRegistrarSolPagoAutorizComponent } from './obs-registrar-sol-pago-autoriz.component';

describe('ObsRegistrarSolPagoAutorizComponent', () => {
  let component: ObsRegistrarSolPagoAutorizComponent;
  let fixture: ComponentFixture<ObsRegistrarSolPagoAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsRegistrarSolPagoAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsRegistrarSolPagoAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
