import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarActaDeInicioFIPreconstruccionComponent } from './validar-acta-de-inicio-f-i-prc.component';

describe('ValidarActaDeInicioFIPreconstruccionComponent', () => {
  let component: ValidarActaDeInicioFIPreconstruccionComponent;
  let fixture: ComponentFixture<ValidarActaDeInicioFIPreconstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarActaDeInicioFIPreconstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarActaDeInicioFIPreconstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
