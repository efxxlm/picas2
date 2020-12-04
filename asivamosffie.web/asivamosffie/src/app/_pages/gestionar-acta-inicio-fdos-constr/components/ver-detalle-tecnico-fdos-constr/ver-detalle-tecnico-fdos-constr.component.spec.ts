import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleTecnicoFdosConstrComponent } from './ver-detalle-tecnico-fdos-constr.component';

describe('VerDetalleTecnicoFdosConstrComponent', () => {
  let component: VerDetalleTecnicoFdosConstrComponent;
  let fixture: ComponentFixture<VerDetalleTecnicoFdosConstrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleTecnicoFdosConstrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleTecnicoFdosConstrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
