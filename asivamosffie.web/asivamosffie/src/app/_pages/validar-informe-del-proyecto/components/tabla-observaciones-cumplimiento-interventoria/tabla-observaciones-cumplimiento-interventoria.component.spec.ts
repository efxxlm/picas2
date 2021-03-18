import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TablaObservacionesCumplimientoInterventoriaComponent } from './tabla-observaciones-cumplimiento-interventoria.component';

describe('TablaObservacionesCumplimientoInterventoriaComponent', () => {
  let component: TablaObservacionesCumplimientoInterventoriaComponent;
  let fixture: ComponentFixture<TablaObservacionesCumplimientoInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaObservacionesCumplimientoInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaObservacionesCumplimientoInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
