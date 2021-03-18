import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGestionarUsuariosComponent } from './tabla-gestionar-usuarios.component';

describe('TablaGestionarUsuariosComponent', () => {
  let component: TablaGestionarUsuariosComponent;
  let fixture: ComponentFixture<TablaGestionarUsuariosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGestionarUsuariosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGestionarUsuariosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
