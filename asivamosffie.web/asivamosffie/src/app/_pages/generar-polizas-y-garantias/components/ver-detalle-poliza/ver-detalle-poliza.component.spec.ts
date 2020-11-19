import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetallePolizaComponent } from './ver-detalle-poliza.component';

describe('VerDetallePolizaComponent', () => {
  let component: VerDetallePolizaComponent;
  let fixture: ComponentFixture<VerDetallePolizaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetallePolizaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetallePolizaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
