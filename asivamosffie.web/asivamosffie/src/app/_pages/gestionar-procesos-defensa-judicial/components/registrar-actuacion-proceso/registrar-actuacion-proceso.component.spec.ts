import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarActuacionProcesoComponent } from './registrar-actuacion-proceso.component';

describe('RegistrarActuacionProcesoComponent', () => {
  let component: RegistrarActuacionProcesoComponent;
  let fixture: ComponentFixture<RegistrarActuacionProcesoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarActuacionProcesoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarActuacionProcesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
