import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleAvanceActuaDerivadasComponent } from './detalle-avance-actua-derivadas.component';

describe('DetalleAvanceActuaDerivadasComponent', () => {
  let component: DetalleAvanceActuaDerivadasComponent;
  let fixture: ComponentFixture<DetalleAvanceActuaDerivadasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleAvanceActuaDerivadasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleAvanceActuaDerivadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
