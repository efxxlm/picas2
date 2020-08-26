import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConDisponibilidadComponent } from './con-disponibilidad.component';

describe('ConDisponibilidadComponent', () => {
  let component: ConDisponibilidadComponent;
  let fixture: ComponentFixture<ConDisponibilidadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConDisponibilidadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConDisponibilidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
