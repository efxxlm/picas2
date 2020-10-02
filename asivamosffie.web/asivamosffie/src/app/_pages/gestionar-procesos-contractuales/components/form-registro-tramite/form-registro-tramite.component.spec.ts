import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistroTramiteComponent } from './form-registro-tramite.component';

describe('FormRegistroTramiteComponent', () => {
  let component: FormRegistroTramiteComponent;
  let fixture: ComponentFixture<FormRegistroTramiteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistroTramiteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistroTramiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
