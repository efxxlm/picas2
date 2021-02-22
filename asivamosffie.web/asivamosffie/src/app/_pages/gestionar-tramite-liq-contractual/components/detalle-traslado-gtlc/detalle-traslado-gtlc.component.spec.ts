import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTrasladoGtlcComponent } from './detalle-traslado-gtlc.component';

describe('DetalleTrasladoGtlcComponent', () => {
  let component: DetalleTrasladoGtlcComponent;
  let fixture: ComponentFixture<DetalleTrasladoGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTrasladoGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTrasladoGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
