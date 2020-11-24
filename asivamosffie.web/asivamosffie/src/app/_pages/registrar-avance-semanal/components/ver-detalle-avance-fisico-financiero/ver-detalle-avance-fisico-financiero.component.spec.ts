import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleAvanceFisicoFinancieroComponent } from './ver-detalle-avance-fisico-financiero.component';

describe('VerDetalleAvanceFisicoFinancieroComponent', () => {
  let component: VerDetalleAvanceFisicoFinancieroComponent;
  let fixture: ComponentFixture<VerDetalleAvanceFisicoFinancieroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleAvanceFisicoFinancieroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleAvanceFisicoFinancieroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
