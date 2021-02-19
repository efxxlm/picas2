import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarNovedadAccordComponent } from './form-registrar-novedad-accord.component';

describe('FormRegistrarNovedadAccordComponent', () => {
  let component: FormRegistrarNovedadAccordComponent;
  let fixture: ComponentFixture<FormRegistrarNovedadAccordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarNovedadAccordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarNovedadAccordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
