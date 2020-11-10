import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleInterventoriaVrtcComponent } from './verdetalle-interventoria-vrtc.component';

describe('VerdetalleInterventoriaVrtcComponent', () => {
  let component: VerdetalleInterventoriaVrtcComponent;
  let fixture: ComponentFixture<VerdetalleInterventoriaVrtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleInterventoriaVrtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleInterventoriaVrtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
