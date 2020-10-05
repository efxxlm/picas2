import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGestionarValidacionComponent } from './tabla-gestionar-validacion.component';

describe('TablaGestionarValidacionComponent', () => {
  let component: TablaGestionarValidacionComponent;
  let fixture: ComponentFixture<TablaGestionarValidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGestionarValidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGestionarValidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
