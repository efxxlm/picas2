import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistarActuacionNotaiComponent } from './form-registar-actuacion-notai.component';

describe('FormRegistarActuacionNotaiComponent', () => {
  let component: FormRegistarActuacionNotaiComponent;
  let fixture: ComponentFixture<FormRegistarActuacionNotaiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistarActuacionNotaiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistarActuacionNotaiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
