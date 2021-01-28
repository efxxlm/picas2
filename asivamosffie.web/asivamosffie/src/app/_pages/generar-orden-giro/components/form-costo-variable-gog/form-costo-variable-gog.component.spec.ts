import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCostoVariableGogComponent } from './form-costo-variable-gog.component';

describe('FormCostoVariableGogComponent', () => {
  let component: FormCostoVariableGogComponent;
  let fixture: ComponentFixture<FormCostoVariableGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCostoVariableGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCostoVariableGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
