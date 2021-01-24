import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleFactProcasValidfspComponent } from './detalle-fact-procas-validfsp.component';

describe('DetalleFactProcasValidfspComponent', () => {
  let component: DetalleFactProcasValidfspComponent;
  let fixture: ComponentFixture<DetalleFactProcasValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleFactProcasValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleFactProcasValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
