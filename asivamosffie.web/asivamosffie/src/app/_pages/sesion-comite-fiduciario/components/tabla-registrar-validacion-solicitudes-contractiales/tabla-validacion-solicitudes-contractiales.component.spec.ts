import { async, ComponentFixture, TestBed } from '@angular/core/testing';

//import { TablaRegistrarValidacionSolicitudesContractialesComponent } from './tabla-registrar-validacion-solicitudes-contractiales.component';
import { TablaRegistrarValidacionSolicitudesContractialesComponent } from './tabla-validacion-solicitudes-contractiales.component';

describe('TablaRegistrarValidacionSolicitudesContractialesComponent', () => {
  let component: TablaRegistrarValidacionSolicitudesContractialesComponent;
  let fixture: ComponentFixture<TablaRegistrarValidacionSolicitudesContractialesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistrarValidacionSolicitudesContractialesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistrarValidacionSolicitudesContractialesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
