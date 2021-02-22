import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleInformeFinalGtlcComponent } from './detalle-informe-final-gtlc.component';

describe('DetalleInformeFinalGtlcComponent', () => {
  let component: DetalleInformeFinalGtlcComponent;
  let fixture: ComponentFixture<DetalleInformeFinalGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleInformeFinalGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleInformeFinalGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
