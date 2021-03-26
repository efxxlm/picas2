import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuFichaProyectoComponent } from './menu-ficha-proyecto.component';

describe('MenuFichaProyectoComponent', () => {
  let component: MenuFichaProyectoComponent;
  let fixture: ComponentFixture<MenuFichaProyectoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MenuFichaProyectoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuFichaProyectoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
