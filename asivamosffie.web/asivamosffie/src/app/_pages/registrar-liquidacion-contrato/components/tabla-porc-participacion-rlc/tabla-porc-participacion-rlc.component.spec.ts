import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaPorcParticipacionRlcComponent } from './tabla-porc-participacion-rlc.component';

describe('TablaPorcParticipacionRlcComponent', () => {
  let component: TablaPorcParticipacionRlcComponent;
  let fixture: ComponentFixture<TablaPorcParticipacionRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaPorcParticipacionRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaPorcParticipacionRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
