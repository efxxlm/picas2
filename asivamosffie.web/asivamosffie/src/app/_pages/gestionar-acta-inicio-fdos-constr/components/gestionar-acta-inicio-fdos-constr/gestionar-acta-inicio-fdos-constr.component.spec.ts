import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarActaInicioFdosConstrComponent } from './gestionar-acta-inicio-fdos-constr.component';

describe('GestionarActaInicioFdosConstrComponent', () => {
  let component: GestionarActaInicioFdosConstrComponent;
  let fixture: ComponentFixture<GestionarActaInicioFdosConstrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarActaInicioFdosConstrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarActaInicioFdosConstrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
