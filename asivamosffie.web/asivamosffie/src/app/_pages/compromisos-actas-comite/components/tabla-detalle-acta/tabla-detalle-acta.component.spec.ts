import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleActaComponent } from './tabla-detalle-acta.component';

describe('TablaDetalleActaComponent', () => {
  let component: TablaDetalleActaComponent;
  let fixture: ComponentFixture<TablaDetalleActaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleActaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleActaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
