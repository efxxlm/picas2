import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDisponibilidadMaterialComponent } from './tabla-disponibilidad-material.component';

describe('TablaDisponibilidadMaterialComponent', () => {
  let component: TablaDisponibilidadMaterialComponent;
  let fixture: ComponentFixture<TablaDisponibilidadMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDisponibilidadMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDisponibilidadMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
