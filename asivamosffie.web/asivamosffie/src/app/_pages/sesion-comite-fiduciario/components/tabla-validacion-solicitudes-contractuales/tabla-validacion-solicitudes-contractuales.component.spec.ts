import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaValidacionSolicitudesContractualesComponent } from './tabla-validacion-solicitudes-contractuales.component';

describe('TablaValidacionSolicitudesContractualesComponent', () => {
  let component: TablaValidacionSolicitudesContractualesComponent;
  let fixture: ComponentFixture<TablaValidacionSolicitudesContractualesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaValidacionSolicitudesContractualesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaValidacionSolicitudesContractualesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
