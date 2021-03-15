import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGestionarParametricasComponent } from './tabla-gestionar-parametricas.component';

describe('TablaGestionarParametricasComponent', () => {
  let component: TablaGestionarParametricasComponent;
  let fixture: ComponentFixture<TablaGestionarParametricasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGestionarParametricasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGestionarParametricasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
