import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConDisponibilidadComponent } from './tabla-con-disponibilidad.component';

describe('TablaConDisponibilidadComponent', () => {
  let component: TablaConDisponibilidadComponent;
  let fixture: ComponentFixture<TablaConDisponibilidadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConDisponibilidadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConDisponibilidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
