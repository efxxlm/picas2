import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleInformeFinalAnexosComponent } from './tabla-detalle-informe-final-anexos.component';

describe('TablaDetalleInformeFinalAnexosComponent', () => {
  let component: TablaDetalleInformeFinalAnexosComponent;
  let fixture: ComponentFixture<TablaDetalleInformeFinalAnexosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleInformeFinalAnexosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleInformeFinalAnexosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
