import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSolicitudExpensasComponent } from './form-solicitud-expensas.component';

describe('FormSolicitudExpensasComponent', () => {
  let component: FormSolicitudExpensasComponent;
  let fixture: ComponentFixture<FormSolicitudExpensasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormSolicitudExpensasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormSolicitudExpensasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
