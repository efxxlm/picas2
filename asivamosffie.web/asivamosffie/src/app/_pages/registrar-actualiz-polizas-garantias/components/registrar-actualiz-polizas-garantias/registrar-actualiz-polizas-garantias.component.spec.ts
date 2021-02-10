import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarActualizPolizasGarantiasComponent } from './registrar-actualiz-polizas-garantias.component';

describe('RegistrarActualizPolizasGarantiasComponent', () => {
  let component: RegistrarActualizPolizasGarantiasComponent;
  let fixture: ComponentFixture<RegistrarActualizPolizasGarantiasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarActualizPolizasGarantiasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarActualizPolizasGarantiasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
