import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConDisponibilidadCanceladaComponent } from './con-disponibilidad-cancelada.component';

describe('ConDisponibilidadCanceladaComponent', () => {
  let component: ConDisponibilidadCanceladaComponent;
  let fixture: ComponentFixture<ConDisponibilidadCanceladaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConDisponibilidadCanceladaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConDisponibilidadCanceladaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
