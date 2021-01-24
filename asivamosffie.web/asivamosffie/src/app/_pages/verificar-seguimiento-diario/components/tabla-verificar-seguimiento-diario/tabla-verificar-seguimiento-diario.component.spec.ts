import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaVerificarSeguimientoDiarioComponent } from './tabla-verificar-seguimiento-diario.component';

describe('TablaVerificarSeguimientoDiarioComponent', () => {
  let component: TablaVerificarSeguimientoDiarioComponent;
  let fixture: ComponentFixture<TablaVerificarSeguimientoDiarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaVerificarSeguimientoDiarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaVerificarSeguimientoDiarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
