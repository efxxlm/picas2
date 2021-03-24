import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidListchequeoVfspComponent } from './form-valid-listchequeo-vfsp.component';

describe('FormValidListchequeoVfspComponent', () => {
  let component: FormValidListchequeoVfspComponent;
  let fixture: ComponentFixture<FormValidListchequeoVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormValidListchequeoVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormValidListchequeoVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
