import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormObservacionInformeFinalComponent } from './form-observacion-informe-final.component';

describe('FormObservacionInformeFinalComponent', () => {
  let component: FormObservacionInformeFinalComponent;
  let fixture: ComponentFixture<FormObservacionInformeFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormObservacionInformeFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormObservacionInformeFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
