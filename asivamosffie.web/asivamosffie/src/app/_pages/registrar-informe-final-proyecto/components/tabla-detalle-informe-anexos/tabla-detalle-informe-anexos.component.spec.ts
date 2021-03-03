import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleInformeAnexosComponent } from './tabla-detalle-informe-anexos.component';

describe('TablaDetalleInformeAnexosComponent', () => {
  let component: TablaDetalleInformeAnexosComponent;
  let fixture: ComponentFixture<TablaDetalleInformeAnexosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleInformeAnexosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleInformeAnexosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
