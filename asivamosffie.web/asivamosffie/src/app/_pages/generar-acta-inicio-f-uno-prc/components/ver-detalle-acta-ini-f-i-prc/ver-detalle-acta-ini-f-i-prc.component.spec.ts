import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleActaIniFIPreconstruccioComponent } from './ver-detalle-acta-ini-f-i-prc.component';

describe('VerDetalleActaIniFIPreconstruccioComponent', () => {
  let component: VerDetalleActaIniFIPreconstruccioComponent;
  let fixture: ComponentFixture<VerDetalleActaIniFIPreconstruccioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleActaIniFIPreconstruccioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleActaIniFIPreconstruccioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
