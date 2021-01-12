import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargarFlujoComponent } from './cargar-flujo.component';

describe('CargarFlujoComponent', () => {
  let component: CargarFlujoComponent;
  let fixture: ComponentFixture<CargarFlujoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargarFlujoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargarFlujoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
