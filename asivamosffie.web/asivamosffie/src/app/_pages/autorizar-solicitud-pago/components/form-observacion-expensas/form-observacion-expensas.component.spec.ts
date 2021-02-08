import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormObservacionExpensasComponent } from './form-observacion-expensas.component';

describe('FormObservacionExpensasComponent', () => {
  let component: FormObservacionExpensasComponent;
  let fixture: ComponentFixture<FormObservacionExpensasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormObservacionExpensasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormObservacionExpensasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
