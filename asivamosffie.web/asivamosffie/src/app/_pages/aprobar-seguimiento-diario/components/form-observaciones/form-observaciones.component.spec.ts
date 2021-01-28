import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormObservacionesComponent } from './form-observaciones.component';

describe('FormObservacionesComponent', () => {
  let component: FormObservacionesComponent;
  let fixture: ComponentFixture<FormObservacionesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormObservacionesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormObservacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
