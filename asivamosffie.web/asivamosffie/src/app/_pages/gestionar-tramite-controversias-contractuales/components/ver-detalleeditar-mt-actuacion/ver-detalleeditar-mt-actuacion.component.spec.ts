import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleeditarMtActuacionComponent } from './ver-detalleeditar-mt-actuacion.component';

describe('VerDetalleeditarMtActuacionComponent', () => {
  let component: VerDetalleeditarMtActuacionComponent;
  let fixture: ComponentFixture<VerDetalleeditarMtActuacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleeditarMtActuacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleeditarMtActuacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
