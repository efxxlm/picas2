import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormNuevoTemaComponent } from './form-nuevo-tema.component';

describe('FormNuevoTemaComponent', () => {
  let component: FormNuevoTemaComponent;
  let fixture: ComponentFixture<FormNuevoTemaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormNuevoTemaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormNuevoTemaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
