import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRecursosCompartidosComponent } from './form-recursos-compartidos.component';

describe('FormRecursosCompartidosComponent', () => {
  let component: FormRecursosCompartidosComponent;
  let fixture: ComponentFixture<FormRecursosCompartidosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRecursosCompartidosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRecursosCompartidosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
