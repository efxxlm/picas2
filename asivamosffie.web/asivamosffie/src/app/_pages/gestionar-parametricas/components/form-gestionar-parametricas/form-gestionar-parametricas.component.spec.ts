import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormGestionarParametricasComponent } from './form-gestionar-parametricas.component';

describe('FormGestionarParametricasComponent', () => {
  let component: FormGestionarParametricasComponent;
  let fixture: ComponentFixture<FormGestionarParametricasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormGestionarParametricasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormGestionarParametricasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
