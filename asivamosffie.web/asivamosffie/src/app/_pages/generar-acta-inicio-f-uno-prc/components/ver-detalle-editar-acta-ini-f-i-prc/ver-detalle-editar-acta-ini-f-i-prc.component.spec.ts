import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleEditarActaIniFIPreconstruccioComponent } from './ver-detalle-editar-acta-ini-f-i-prc.component';

describe('VerDetalleEditarActaIniFIPreconstruccioComponent', () => {
  let component: VerDetalleEditarActaIniFIPreconstruccioComponent;
  let fixture: ComponentFixture<VerDetalleEditarActaIniFIPreconstruccioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleEditarActaIniFIPreconstruccioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleEditarActaIniFIPreconstruccioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
