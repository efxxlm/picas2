import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleHojasVidaContratistaComponent } from './detalle-hojas-vida-contratista.component';

describe('DetalleHojasVidaContratistaComponent', () => {
  let component: DetalleHojasVidaContratistaComponent;
  let fixture: ComponentFixture<DetalleHojasVidaContratistaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleHojasVidaContratistaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleHojasVidaContratistaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
