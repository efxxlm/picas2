import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaContratoDeInterventoriaComponent } from './tabla-contrato-de-interventoria.component';

describe('TablaContratoDeInterventoriaComponent', () => {
  let component: TablaContratoDeInterventoriaComponent;
  let fixture: ComponentFixture<TablaContratoDeInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaContratoDeInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaContratoDeInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
