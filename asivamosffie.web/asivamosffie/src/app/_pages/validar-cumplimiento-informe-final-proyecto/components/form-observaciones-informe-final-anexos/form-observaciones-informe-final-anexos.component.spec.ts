import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormObservacionesInformeFinalAnexosComponent } from './form-observaciones-informe-final-anexos.component';

describe('FormObservacionesInformeFinalAnexosComponent', () => {
  let component: FormObservacionesInformeFinalAnexosComponent;
  let fixture: ComponentFixture<FormObservacionesInformeFinalAnexosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormObservacionesInformeFinalAnexosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormObservacionesInformeFinalAnexosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
