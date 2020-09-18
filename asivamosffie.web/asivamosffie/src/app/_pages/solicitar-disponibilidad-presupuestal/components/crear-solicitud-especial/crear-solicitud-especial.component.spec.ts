import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrearSolicitudEspecialComponent } from './crear-solicitud-especial.component';

describe('CrearSolicitudEspecialComponent', () => {
  let component: CrearSolicitudEspecialComponent;
  let fixture: ComponentFixture<CrearSolicitudEspecialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrearSolicitudEspecialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrearSolicitudEspecialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
