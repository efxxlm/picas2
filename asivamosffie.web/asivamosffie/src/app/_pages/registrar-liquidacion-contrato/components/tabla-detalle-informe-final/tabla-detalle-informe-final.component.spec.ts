import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleInformeFinalComponent } from './tabla-detalle-informe-final.component';

describe('TablaDetalleInformeFinalComponent', () => {
  let component: TablaDetalleInformeFinalComponent;
  let fixture: ComponentFixture<TablaDetalleInformeFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleInformeFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleInformeFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
