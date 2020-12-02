import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRequisitosTecnicosConstruccionComponent } from './form-requisitos-tecnicos-construccion.component';

describe('FormRequisitosTecnicosConstruccionComponent', () => {
  let component: FormRequisitosTecnicosConstruccionComponent;
  let fixture: ComponentFixture<FormRequisitosTecnicosConstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRequisitosTecnicosConstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRequisitosTecnicosConstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
