import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetallePolizaRapgComponent } from './ver-detalle-poliza-rapg.component';

describe('VerDetallePolizaRapgComponent', () => {
  let component: VerDetallePolizaRapgComponent;
  let fixture: ComponentFixture<VerDetallePolizaRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetallePolizaRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetallePolizaRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
