import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormEjecucionFinancieraComponent } from './form-ejecucion-financiera.component';

describe('FormEjecucionFinancieraComponent', () => {
  let component: FormEjecucionFinancieraComponent;
  let fixture: ComponentFixture<FormEjecucionFinancieraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormEjecucionFinancieraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormEjecucionFinancieraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
