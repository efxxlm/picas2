import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DescuentosDireccionTecnicaComponent } from './descuentos-direccion-tecnica.component';

describe('DescuentosDireccionTecnicaComponent', () => {
  let component: DescuentosDireccionTecnicaComponent;
  let fixture: ComponentFixture<DescuentosDireccionTecnicaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DescuentosDireccionTecnicaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DescuentosDireccionTecnicaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
