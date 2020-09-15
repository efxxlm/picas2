import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormLiquidacionComponent } from './form-liquidacion.component';

describe('FormLiquidacionComponent', () => {
  let component: FormLiquidacionComponent;
  let fixture: ComponentFixture<FormLiquidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormLiquidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormLiquidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
