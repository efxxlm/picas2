import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerarActaInicioFaseunoPreconstruccionComponent } from './generar-acta-inicio-f-i-prc.component';

describe('GenerarActaInicioFaseunoPreconstruccionComponent', () => {
  let component: GenerarActaInicioFaseunoPreconstruccionComponent;
  let fixture: ComponentFixture<GenerarActaInicioFaseunoPreconstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerarActaInicioFaseunoPreconstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerarActaInicioFaseunoPreconstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
