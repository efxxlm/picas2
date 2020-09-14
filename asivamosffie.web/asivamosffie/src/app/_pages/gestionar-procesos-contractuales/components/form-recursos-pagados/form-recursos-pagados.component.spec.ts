import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRecursosPagadosComponent } from './form-recursos-pagados.component';

describe('FormRecursosPagadosComponent', () => {
  let component: FormRecursosPagadosComponent;
  let fixture: ComponentFixture<FormRecursosPagadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRecursosPagadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRecursosPagadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
