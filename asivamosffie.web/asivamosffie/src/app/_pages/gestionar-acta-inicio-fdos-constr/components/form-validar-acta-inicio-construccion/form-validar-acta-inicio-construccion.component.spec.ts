import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidarActaInicioConstruccionComponent } from './form-validar-acta-inicio-construccion.component';

describe('FormValidarActaInicioConstruccionComponent', () => {
  let component: FormValidarActaInicioConstruccionComponent;
  let fixture: ComponentFixture<FormValidarActaInicioConstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormValidarActaInicioConstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormValidarActaInicioConstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
