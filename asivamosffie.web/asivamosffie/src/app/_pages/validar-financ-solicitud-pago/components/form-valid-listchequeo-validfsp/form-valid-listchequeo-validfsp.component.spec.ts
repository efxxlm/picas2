import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidListchequeoValidfspComponent } from './form-valid-listchequeo-validfsp.component';

describe('FormValidListchequeoValidfspComponent', () => {
  let component: FormValidListchequeoValidfspComponent;
  let fixture: ComponentFixture<FormValidListchequeoValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormValidListchequeoValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormValidListchequeoValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
