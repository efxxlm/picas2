import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormConvocadosPasivaDjComponent } from './form-convocados-pasiva-dj.component';

describe('FormConvocadosPasivaDjComponent', () => {
  let component: FormConvocadosPasivaDjComponent;
  let fixture: ComponentFixture<FormConvocadosPasivaDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormConvocadosPasivaDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormConvocadosPasivaDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
