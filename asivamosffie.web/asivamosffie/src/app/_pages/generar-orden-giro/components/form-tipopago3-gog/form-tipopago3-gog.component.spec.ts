import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormTipopago3GogComponent } from './form-tipopago3-gog.component';

describe('FormTipopago3GogComponent', () => {
  let component: FormTipopago3GogComponent;
  let fixture: ComponentFixture<FormTipopago3GogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormTipopago3GogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormTipopago3GogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
