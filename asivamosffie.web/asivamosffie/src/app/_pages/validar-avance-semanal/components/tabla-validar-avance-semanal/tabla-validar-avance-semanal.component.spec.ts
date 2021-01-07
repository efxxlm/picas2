import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaValidarAvanceSemanalComponent } from './tabla-validar-avance-semanal.component';

describe('TablaValidarAvanceSemanalComponent', () => {
  let component: TablaValidarAvanceSemanalComponent;
  let fixture: ComponentFixture<TablaValidarAvanceSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaValidarAvanceSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaValidarAvanceSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
