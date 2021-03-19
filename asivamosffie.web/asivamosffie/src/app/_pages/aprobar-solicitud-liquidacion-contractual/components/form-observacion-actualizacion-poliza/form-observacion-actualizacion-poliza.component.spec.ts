import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormObservacionActualizacionPolizaComponent } from './form-observacion-actualizacion-poliza.component';

describe('FormObservacionActualizacionPolizaComponent', () => {
  let component: FormObservacionActualizacionPolizaComponent;
  let fixture: ComponentFixture<FormObservacionActualizacionPolizaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormObservacionActualizacionPolizaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormObservacionActualizacionPolizaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
