import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTablaFlujoRecursosComponent } from './detalle-tabla-flujo-recursos.component';

describe('DetalleTablaFlujoRecursosComponent', () => {
  let component: DetalleTablaFlujoRecursosComponent;
  let fixture: ComponentFixture<DetalleTablaFlujoRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTablaFlujoRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTablaFlujoRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
