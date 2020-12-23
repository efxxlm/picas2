import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleContratoObraArtcComponent } from './ver-detalle-contrato-obra-artc.component';

describe('VerDetalleContratoObraArtcComponent', () => {
  let component: VerDetalleContratoObraArtcComponent;
  let fixture: ComponentFixture<VerDetalleContratoObraArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleContratoObraArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleContratoObraArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
