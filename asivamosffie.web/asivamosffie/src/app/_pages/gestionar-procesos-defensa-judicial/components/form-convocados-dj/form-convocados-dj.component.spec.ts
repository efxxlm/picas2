import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormConvocadosDjComponent } from './form-convocados-dj.component';

describe('FormConvocadosDjComponent', () => {
  let component: FormConvocadosDjComponent;
  let fixture: ComponentFixture<FormConvocadosDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormConvocadosDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormConvocadosDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
