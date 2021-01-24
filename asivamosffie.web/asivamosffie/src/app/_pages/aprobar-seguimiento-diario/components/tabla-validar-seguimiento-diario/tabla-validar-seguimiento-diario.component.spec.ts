import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaValidarSeguimientoDiarioComponent } from './tabla-validar-seguimiento-diario.component';

describe('TablaValidarSeguimientoDiarioComponent', () => {
  let component: TablaValidarSeguimientoDiarioComponent;
  let fixture: ComponentFixture<TablaValidarSeguimientoDiarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaValidarSeguimientoDiarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaValidarSeguimientoDiarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
