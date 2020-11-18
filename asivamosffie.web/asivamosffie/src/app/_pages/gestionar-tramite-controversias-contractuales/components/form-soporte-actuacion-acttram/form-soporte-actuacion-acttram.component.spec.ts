import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSoporteActuacionActtramComponent } from './form-soporte-actuacion-acttram.component';

describe('FormSoporteActuacionActtramComponent', () => {
  let component: FormSoporteActuacionActtramComponent;
  let fixture: ComponentFixture<FormSoporteActuacionActtramComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormSoporteActuacionActtramComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormSoporteActuacionActtramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
