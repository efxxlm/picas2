import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleReqTecConstrComponent } from './ver-detalle-req-tec-constr.component';

describe('VerDetalleReqTecConstrComponent', () => {
  let component: VerDetalleReqTecConstrComponent;
  let fixture: ComponentFixture<VerDetalleReqTecConstrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleReqTecConstrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleReqTecConstrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
