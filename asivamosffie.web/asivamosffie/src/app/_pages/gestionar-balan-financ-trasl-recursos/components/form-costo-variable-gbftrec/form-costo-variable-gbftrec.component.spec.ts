import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCostoVariableGbftrecComponent } from './form-costo-variable-gbftrec.component';

describe('FormCostoVariableGbftrecComponent', () => {
  let component: FormCostoVariableGbftrecComponent;
  let fixture: ComponentFixture<FormCostoVariableGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCostoVariableGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCostoVariableGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
