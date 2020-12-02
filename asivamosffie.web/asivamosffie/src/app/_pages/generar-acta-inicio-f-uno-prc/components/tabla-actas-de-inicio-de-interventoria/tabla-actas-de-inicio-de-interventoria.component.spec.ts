import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaActasDeInicioDeInterventoriaComponent } from './tabla-actas-de-inicio-de-interventoria.component';

describe('TablaActasDeInicioDeInterventoriaComponent', () => {
  let component: TablaActasDeInicioDeInterventoriaComponent;
  let fixture: ComponentFixture<TablaActasDeInicioDeInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaActasDeInicioDeInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaActasDeInicioDeInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
