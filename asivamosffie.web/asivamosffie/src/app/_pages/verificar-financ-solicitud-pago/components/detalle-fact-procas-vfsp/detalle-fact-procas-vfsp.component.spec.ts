import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleFactProcasVfspComponent } from './detalle-fact-procas-vfsp.component';

describe('DetalleFactProcasVfspComponent', () => {
  let component: DetalleFactProcasVfspComponent;
  let fixture: ComponentFixture<DetalleFactProcasVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleFactProcasVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleFactProcasVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
