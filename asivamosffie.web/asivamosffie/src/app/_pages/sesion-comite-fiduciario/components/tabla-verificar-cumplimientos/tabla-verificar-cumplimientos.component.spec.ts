import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaVerificarCumplimientosComponent } from './tabla-verificar-cumplimientos.component';

describe('TablaVerificarCumplimientosComponent', () => {
  let component: TablaVerificarCumplimientosComponent;
  let fixture: ComponentFixture<TablaVerificarCumplimientosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaVerificarCumplimientosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaVerificarCumplimientosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
