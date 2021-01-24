import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSoporteSolicitudUrlComponent } from './form-soporte-solicitud-url.component';

describe('FormSoporteSolicitudUrlComponent', () => {
  let component: FormSoporteSolicitudUrlComponent;
  let fixture: ComponentFixture<FormSoporteSolicitudUrlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormSoporteSolicitudUrlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormSoporteSolicitudUrlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
