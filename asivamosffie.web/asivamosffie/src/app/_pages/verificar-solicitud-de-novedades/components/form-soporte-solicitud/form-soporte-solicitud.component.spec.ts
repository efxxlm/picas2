import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSoporteSolicitudComponent } from './form-soporte-solicitud.component';

describe('FormSoporteSolicitudComponent', () => {
  let component: FormSoporteSolicitudComponent;
  let fixture: ComponentFixture<FormSoporteSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormSoporteSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormSoporteSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
