import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormTipopago3TercGogComponent } from './form-tipopago3-terc-gog.component';

describe('FormTipopago3TercGogComponent', () => {
  let component: FormTipopago3TercGogComponent;
  let fixture: ComponentFixture<FormTipopago3TercGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormTipopago3TercGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormTipopago3TercGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
