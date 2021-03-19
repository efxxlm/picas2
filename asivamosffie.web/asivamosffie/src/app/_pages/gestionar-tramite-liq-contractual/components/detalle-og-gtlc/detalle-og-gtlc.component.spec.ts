import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleOgGtlcComponent } from './detalle-og-gtlc.component';

describe('DetalleOgGtlcComponent', () => {
  let component: DetalleOgGtlcComponent;
  let fixture: ComponentFixture<DetalleOgGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleOgGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleOgGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
