import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistrarAvanceSemanalComponent } from './tabla-registrar-avance-semanal.component';

describe('TablaRegistrarAvanceSemanalComponent', () => {
  let component: TablaRegistrarAvanceSemanalComponent;
  let fixture: ComponentFixture<TablaRegistrarAvanceSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistrarAvanceSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistrarAvanceSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
