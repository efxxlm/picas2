import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormOtrosTemasComponent } from './form-otros-temas.component';

describe('FormOtrosTemasComponent', () => {
  let component: FormOtrosTemasComponent;
  let fixture: ComponentFixture<FormOtrosTemasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormOtrosTemasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormOtrosTemasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
