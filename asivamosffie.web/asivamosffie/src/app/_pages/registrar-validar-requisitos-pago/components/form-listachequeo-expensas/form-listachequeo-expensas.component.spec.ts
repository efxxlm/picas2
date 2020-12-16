import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormListachequeoExpensasComponent } from './form-listachequeo-expensas.component';

describe('FormListachequeoExpensasComponent', () => {
  let component: FormListachequeoExpensasComponent;
  let fixture: ComponentFixture<FormListachequeoExpensasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormListachequeoExpensasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormListachequeoExpensasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
