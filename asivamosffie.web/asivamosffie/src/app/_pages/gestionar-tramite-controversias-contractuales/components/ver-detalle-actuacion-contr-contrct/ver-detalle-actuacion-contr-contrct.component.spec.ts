import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleActuacionContrContrctComponent } from './ver-detalle-actuacion-contr-contrct.component';

describe('VerDetalleActuacionContrContrctComponent', () => {
  let component: VerDetalleActuacionContrContrctComponent;
  let fixture: ComponentFixture<VerDetalleActuacionContrContrctComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleActuacionContrContrctComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleActuacionContrContrctComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
