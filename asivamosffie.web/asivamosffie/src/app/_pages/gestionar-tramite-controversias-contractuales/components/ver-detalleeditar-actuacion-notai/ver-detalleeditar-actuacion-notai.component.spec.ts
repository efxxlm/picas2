import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleeditarActuacionNotaiComponent } from './ver-detalleeditar-actuacion-notai.component';

describe('VerDetalleeditarActuacionNotaiComponent', () => {
  let component: VerDetalleeditarActuacionNotaiComponent;
  let fixture: ComponentFixture<VerDetalleeditarActuacionNotaiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleeditarActuacionNotaiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleeditarActuacionNotaiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
