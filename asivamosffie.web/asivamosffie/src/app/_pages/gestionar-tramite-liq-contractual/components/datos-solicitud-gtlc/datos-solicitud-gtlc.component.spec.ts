import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosSolicitudGtlcComponent } from './datos-solicitud-gtlc.component';

describe('DatosSolicitudGtlcComponent', () => {
  let component: DatosSolicitudGtlcComponent;
  let fixture: ComponentFixture<DatosSolicitudGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatosSolicitudGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatosSolicitudGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
