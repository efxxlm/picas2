import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGestionCompromisosComponent } from './tabla-gestion-compromisos.component';

describe('TablaGestionCompromisosComponent', () => {
  let component: TablaGestionCompromisosComponent;
  let fixture: ComponentFixture<TablaGestionCompromisosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGestionCompromisosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGestionCompromisosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
