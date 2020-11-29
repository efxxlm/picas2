import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaContratosInterventoriaVrtcComponent } from './tabla-contratos-interventoria-vrtc.component';

describe('TablaContratosInterventoriaVrtcComponent', () => {
  let component: TablaContratosInterventoriaVrtcComponent;
  let fixture: ComponentFixture<TablaContratosInterventoriaVrtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaContratosInterventoriaVrtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaContratosInterventoriaVrtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
