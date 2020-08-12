import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargarOrdenDeElegibilidadComponent } from './cargar-orden-de-elegibilidad.component';

describe('CargarOrdenDeElegibilidadComponent', () => {
  let component: CargarOrdenDeElegibilidadComponent;
  let fixture: ComponentFixture<CargarOrdenDeElegibilidadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargarOrdenDeElegibilidadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargarOrdenDeElegibilidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
