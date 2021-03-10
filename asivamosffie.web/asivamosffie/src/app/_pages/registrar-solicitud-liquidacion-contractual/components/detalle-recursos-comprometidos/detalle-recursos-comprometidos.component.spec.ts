import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleRecursosComprometidosComponent } from './detalle-recursos-comprometidos.component';

describe('DetalleRecursosComprometidosComponent', () => {
  let component: DetalleRecursosComprometidosComponent;
  let fixture: ComponentFixture<DetalleRecursosComprometidosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleRecursosComprometidosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleRecursosComprometidosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
