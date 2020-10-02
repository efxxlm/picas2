import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleCompromisoComponent } from './tabla-detalle-compromiso.component';

describe('TablaDetalleCompromisoComponent', () => {
  let component: TablaDetalleCompromisoComponent;
  let fixture: ComponentFixture<TablaDetalleCompromisoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleCompromisoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleCompromisoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
