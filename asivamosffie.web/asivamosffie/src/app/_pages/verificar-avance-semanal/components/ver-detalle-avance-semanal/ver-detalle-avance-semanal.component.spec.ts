import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleAvanceSemanalComponent } from './ver-detalle-avance-semanal.component';

describe('VerDetalleAvanceSemanalComponent', () => {
  let component: VerDetalleAvanceSemanalComponent;
  let fixture: ComponentFixture<VerDetalleAvanceSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleAvanceSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleAvanceSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
