import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesActualizacionPolizaComponent } from './observaciones-actualizacion-poliza.component';

describe('ObservacionesActualizacionPolizaComponent', () => {
  let component: ObservacionesActualizacionPolizaComponent;
  let fixture: ComponentFixture<ObservacionesActualizacionPolizaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesActualizacionPolizaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesActualizacionPolizaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
