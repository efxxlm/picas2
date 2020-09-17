import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaVerificarCumplimientoComponent } from './tabla-verificar-cumplimiento.component';

describe('TablaVerificarCumplimientoComponent', () => {
  let component: TablaVerificarCumplimientoComponent;
  let fixture: ComponentFixture<TablaVerificarCumplimientoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaVerificarCumplimientoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaVerificarCumplimientoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
