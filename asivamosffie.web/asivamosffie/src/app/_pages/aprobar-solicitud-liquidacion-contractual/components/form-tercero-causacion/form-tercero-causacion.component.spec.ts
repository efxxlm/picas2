import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormTerceroCausacionComponent } from './form-tercero-causacion.component';

describe('FormTerceroCausacionComponent', () => {
  let component: FormTerceroCausacionComponent;
  let fixture: ComponentFixture<FormTerceroCausacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormTerceroCausacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormTerceroCausacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
