import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormContratosAsociadosDjComponent } from './form-contratos-asociados-dj.component';

describe('FormContratosAsociadosDjComponent', () => {
  let component: FormContratosAsociadosDjComponent;
  let fixture: ComponentFixture<FormContratosAsociadosDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormContratosAsociadosDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormContratosAsociadosDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
