import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuGenerarDisponibilidadComponent } from './menu-generar-disponibilidad.component';

describe('MenuGenerarDisponibilidadComponent', () => {
  let component: MenuGenerarDisponibilidadComponent;
  let fixture: ComponentFixture<MenuGenerarDisponibilidadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MenuGenerarDisponibilidadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuGenerarDisponibilidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
