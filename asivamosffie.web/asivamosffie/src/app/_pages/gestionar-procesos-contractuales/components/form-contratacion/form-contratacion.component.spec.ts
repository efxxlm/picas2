import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormContratacionComponent } from './form-contratacion.component';

describe('FormContratacionComponent', () => {
  let component: FormContratacionComponent;
  let fixture: ComponentFixture<FormContratacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormContratacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormContratacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
