import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleInformeFinalRlcComponent } from './detalle-informe-final-rlc.component';

describe('DetalleInformeFinalRlcComponent', () => {
  let component: DetalleInformeFinalRlcComponent;
  let fixture: ComponentFixture<DetalleInformeFinalRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleInformeFinalRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleInformeFinalRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
