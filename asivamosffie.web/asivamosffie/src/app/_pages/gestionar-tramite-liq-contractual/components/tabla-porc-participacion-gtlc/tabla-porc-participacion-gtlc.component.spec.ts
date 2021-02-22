import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaPorcParticipacionGtlcComponent } from './tabla-porc-participacion-gtlc.component';

describe('TablaPorcParticipacionGtlcComponent', () => {
  let component: TablaPorcParticipacionGtlcComponent;
  let fixture: ComponentFixture<TablaPorcParticipacionGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaPorcParticipacionGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaPorcParticipacionGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
