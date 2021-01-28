import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCostoVariableTercGogComponent } from './form-costo-variable-terc-gog.component';

describe('FormCostoVariableTercGogComponent', () => {
  let component: FormCostoVariableTercGogComponent;
  let fixture: ComponentFixture<FormCostoVariableTercGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCostoVariableTercGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCostoVariableTercGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
