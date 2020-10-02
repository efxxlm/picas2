import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConDisponibilidadCanceladaComponent } from './tabla-con-disponibilidad-cancelada.component';

describe('TablaConDisponibilidadCanceladaComponent', () => {
  let component: TablaConDisponibilidadCanceladaComponent;
  let fixture: ComponentFixture<TablaConDisponibilidadCanceladaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConDisponibilidadCanceladaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConDisponibilidadCanceladaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
