import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargarProgramacionComponent } from './cargar-programacion.component';

describe('CargarProgramacionComponent', () => {
  let component: CargarProgramacionComponent;
  let fixture: ComponentFixture<CargarProgramacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargarProgramacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargarProgramacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
