import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaVerificarAvanceSemanalComponent } from './tabla-verificar-avance-semanal.component';

describe('TablaVerificarAvanceSemanalComponent', () => {
  let component: TablaVerificarAvanceSemanalComponent;
  let fixture: ComponentFixture<TablaVerificarAvanceSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaVerificarAvanceSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaVerificarAvanceSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
