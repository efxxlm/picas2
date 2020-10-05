import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaCrearSolicitudEspecialComponent } from './tabla-crear-solicitud-especial.component';

describe('TablaCrearSolicitudEspecialComponent', () => {
  let component: TablaCrearSolicitudEspecialComponent;
  let fixture: ComponentFixture<TablaCrearSolicitudEspecialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaCrearSolicitudEspecialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaCrearSolicitudEspecialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
