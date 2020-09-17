import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaCrearSolicitudadministrativaComponent } from './tabla-crear-solicitud-administrativa.component';

describe('TablaCrearSolicitudadministrativaComponent', () => {
  let component: TablaCrearSolicitudadministrativaComponent;
  let fixture: ComponentFixture<TablaCrearSolicitudadministrativaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaCrearSolicitudadministrativaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaCrearSolicitudadministrativaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
