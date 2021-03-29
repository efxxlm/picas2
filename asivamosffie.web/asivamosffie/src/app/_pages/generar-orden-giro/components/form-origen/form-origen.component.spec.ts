import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormOrigenComponent } from './form-origen.component';

describe('FormOrigenComponent', () => {
  let component: FormOrigenComponent;
  let fixture: ComponentFixture<FormOrigenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormOrigenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormOrigenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
