import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaMonitoreoCompromisosComponent } from './tabla-monitoreo-compromisos.component';

describe('TablaMonitoreoCompromisosComponent', () => {
  let component: TablaMonitoreoCompromisosComponent;
  let fixture: ComponentFixture<TablaMonitoreoCompromisosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaMonitoreoCompromisosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaMonitoreoCompromisosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
