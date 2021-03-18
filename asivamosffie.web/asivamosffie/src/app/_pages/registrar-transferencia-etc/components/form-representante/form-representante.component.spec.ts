import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRepresentanteComponent } from './form-representante.component';

describe('FormRepresentanteComponent', () => {
  let component: FormRepresentanteComponent;
  let fixture: ComponentFixture<FormRepresentanteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRepresentanteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRepresentanteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
