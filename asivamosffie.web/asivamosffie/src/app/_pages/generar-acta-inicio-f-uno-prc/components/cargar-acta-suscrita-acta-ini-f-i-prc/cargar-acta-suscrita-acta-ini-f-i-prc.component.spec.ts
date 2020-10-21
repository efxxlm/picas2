import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargarActaSuscritaActaIniFIPreconstruccionComponent } from './cargar-acta-suscrita-acta-ini-f-i-prc.component';

describe('CargarActaSuscritaActaIniFIPreconstruccionComponent', () => {
  let component: CargarActaSuscritaActaIniFIPreconstruccionComponent;
  let fixture: ComponentFixture<CargarActaSuscritaActaIniFIPreconstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargarActaSuscritaActaIniFIPreconstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargarActaSuscritaActaIniFIPreconstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
