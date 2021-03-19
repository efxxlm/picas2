import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleEditarPolizaRapgComponent } from './ver-detalle-editar-poliza-rapg.component';

describe('VerDetalleEditarPolizaRapgComponent', () => {
  let component: VerDetalleEditarPolizaRapgComponent;
  let fixture: ComponentFixture<VerDetalleEditarPolizaRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleEditarPolizaRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleEditarPolizaRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
