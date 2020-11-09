import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarControvrsSopSolComponent } from './form-registrar-controvrs-sop-sol.component';

describe('FormRegistrarControvrsSopSolComponent', () => {
  let component: FormRegistrarControvrsSopSolComponent;
  let fixture: ComponentFixture<FormRegistrarControvrsSopSolComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarControvrsSopSolComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarControvrsSopSolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
