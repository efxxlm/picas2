import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleActuacionNotaiComponent } from './ver-detalle-actuacion-notai.component';

describe('VerDetalleActuacionNotaiComponent', () => {
  let component: VerDetalleActuacionNotaiComponent;
  let fixture: ComponentFixture<VerDetalleActuacionNotaiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleActuacionNotaiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleActuacionNotaiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
