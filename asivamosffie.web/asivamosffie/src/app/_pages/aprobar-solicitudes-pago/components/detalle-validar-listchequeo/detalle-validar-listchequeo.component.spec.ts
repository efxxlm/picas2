import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleValidarListchequeoComponent } from './detalle-validar-listchequeo.component';

describe('DetalleValidarListchequeoComponent', () => {
  let component: DetalleValidarListchequeoComponent;
  let fixture: ComponentFixture<DetalleValidarListchequeoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleValidarListchequeoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleValidarListchequeoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
