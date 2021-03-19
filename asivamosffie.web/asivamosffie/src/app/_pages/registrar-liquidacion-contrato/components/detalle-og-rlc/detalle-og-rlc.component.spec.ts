import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleOgRlcComponent } from './detalle-og-rlc.component';

describe('DetalleOgRlcComponent', () => {
  let component: DetalleOgRlcComponent;
  let fixture: ComponentFixture<DetalleOgRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleOgRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleOgRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
