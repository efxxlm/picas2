import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTablaFlujIntRecComponent } from './detalle-tabla-fluj-int-rec.component';

describe('DetalleTablaFlujIntRecComponent', () => {
  let component: DetalleTablaFlujIntRecComponent;
  let fixture: ComponentFixture<DetalleTablaFlujIntRecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTablaFlujIntRecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTablaFlujIntRecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
