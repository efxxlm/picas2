import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleValidarListachequeoComponent } from './tabla-detalle-validar-listachequeo.component';

describe('TablaDetalleValidarListachequeoComponent', () => {
  let component: TablaDetalleValidarListachequeoComponent;
  let fixture: ComponentFixture<TablaDetalleValidarListachequeoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleValidarListachequeoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleValidarListachequeoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
