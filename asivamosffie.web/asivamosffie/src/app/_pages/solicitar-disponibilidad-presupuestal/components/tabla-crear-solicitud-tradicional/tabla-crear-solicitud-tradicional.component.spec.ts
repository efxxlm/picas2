import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaCrearSolicitudTradicionalComponent } from './tabla-crear-solicitud-tradicional.component';

describe('TablaCrearSolicitudTradicionalComponent', () => {
  let component: TablaCrearSolicitudTradicionalComponent;
  let fixture: ComponentFixture<TablaCrearSolicitudTradicionalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaCrearSolicitudTradicionalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaCrearSolicitudTradicionalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
