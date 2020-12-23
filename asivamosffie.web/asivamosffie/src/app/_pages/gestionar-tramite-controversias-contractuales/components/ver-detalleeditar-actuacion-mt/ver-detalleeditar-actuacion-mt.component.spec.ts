import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleeditarActuacionMtComponent } from './ver-detalleeditar-actuacion-mt.component';

describe('VerDetalleeditarActuacionMtComponent', () => {
  let component: VerDetalleeditarActuacionMtComponent;
  let fixture: ComponentFixture<VerDetalleeditarActuacionMtComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleeditarActuacionMtComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleeditarActuacionMtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
