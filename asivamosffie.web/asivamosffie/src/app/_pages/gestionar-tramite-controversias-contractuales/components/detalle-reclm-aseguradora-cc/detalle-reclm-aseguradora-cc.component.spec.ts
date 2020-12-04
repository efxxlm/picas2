import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleReclmAseguradoraCcComponent } from './detalle-reclm-aseguradora-cc.component';

describe('DetalleReclmAseguradoraCcComponent', () => {
  let component: DetalleReclmAseguradoraCcComponent;
  let fixture: ComponentFixture<DetalleReclmAseguradoraCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleReclmAseguradoraCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleReclmAseguradoraCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
