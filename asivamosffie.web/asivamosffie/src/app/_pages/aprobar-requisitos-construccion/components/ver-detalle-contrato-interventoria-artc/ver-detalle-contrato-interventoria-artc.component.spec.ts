import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleContratoInterventoriaArtcComponent } from './ver-detalle-contrato-interventoria-artc.component';

describe('VerDetalleContratoInterventoriaArtcComponent', () => {
  let component: VerDetalleContratoInterventoriaArtcComponent;
  let fixture: ComponentFixture<VerDetalleContratoInterventoriaArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleContratoInterventoriaArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleContratoInterventoriaArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
