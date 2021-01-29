import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleInformeFinalComponent } from './ver-detalle-informe-final.component';

describe('VerDetalleInformeFinalComponent', () => {
  let component: VerDetalleInformeFinalComponent;
  let fixture: ComponentFixture<VerDetalleInformeFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleInformeFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleInformeFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
