import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsCargarFormaPagoComponent } from './obs-cargar-forma-pago.component';

describe('ObsCargarFormaPagoComponent', () => {
  let component: ObsCargarFormaPagoComponent;
  let fixture: ComponentFixture<ObsCargarFormaPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsCargarFormaPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsCargarFormaPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
