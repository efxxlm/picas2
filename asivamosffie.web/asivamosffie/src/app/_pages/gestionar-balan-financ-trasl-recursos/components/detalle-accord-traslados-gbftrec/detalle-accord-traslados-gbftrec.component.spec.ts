import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleAccordTrasladosGbftrecComponent } from './detalle-accord-traslados-gbftrec.component';

describe('DetalleAccordTrasladosGbftrecComponent', () => {
  let component: DetalleAccordTrasladosGbftrecComponent;
  let fixture: ComponentFixture<DetalleAccordTrasladosGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleAccordTrasladosGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleAccordTrasladosGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
