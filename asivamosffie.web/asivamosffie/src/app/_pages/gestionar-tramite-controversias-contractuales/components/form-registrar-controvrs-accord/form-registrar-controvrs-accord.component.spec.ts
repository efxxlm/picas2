import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarControvrsAccordComponent } from './form-registrar-controvrs-accord.component';

describe('FormRegistrarControvrsAccordComponent', () => {
  let component: FormRegistrarControvrsAccordComponent;
  let fixture: ComponentFixture<FormRegistrarControvrsAccordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarControvrsAccordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarControvrsAccordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
