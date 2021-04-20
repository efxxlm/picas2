import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleRegistroComponent } from './ver-detalle-registro.component';

describe('VerDetalleRegistroComponent', () => {
  let component: VerDetalleRegistroComponent;
  let fixture: ComponentFixture<VerDetalleRegistroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleRegistroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleRegistroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
