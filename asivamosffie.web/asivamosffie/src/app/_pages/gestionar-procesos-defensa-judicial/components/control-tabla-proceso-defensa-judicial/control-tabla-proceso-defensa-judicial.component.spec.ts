import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlTablaProcesoDefensaJudicialComponent } from './control-tabla-proceso-defensa-judicial.component';

describe('ControlTablaProcesoDefensaJudicialComponent', () => {
  let component: ControlTablaProcesoDefensaJudicialComponent;
  let fixture: ComponentFixture<ControlTablaProcesoDefensaJudicialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlTablaProcesoDefensaJudicialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlTablaProcesoDefensaJudicialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
