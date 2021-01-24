import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleeditarActuacionReclmComponent } from './ver-detalleeditar-actuacion-reclm.component';

describe('VerDetalleeditarActuacionReclmComponent', () => {
  let component: VerDetalleeditarActuacionReclmComponent;
  let fixture: ComponentFixture<VerDetalleeditarActuacionReclmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleeditarActuacionReclmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleeditarActuacionReclmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
