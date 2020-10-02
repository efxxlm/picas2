import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerarActaFdosConstrComponent } from './generar-acta-fdos-constr.component';

describe('GenerarActaFdosConstrComponent', () => {
  let component: GenerarActaFdosConstrComponent;
  let fixture: ComponentFixture<GenerarActaFdosConstrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerarActaFdosConstrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerarActaFdosConstrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
