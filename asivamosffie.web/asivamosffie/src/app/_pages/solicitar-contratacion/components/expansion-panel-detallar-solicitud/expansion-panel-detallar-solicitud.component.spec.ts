import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpansionPanelDetallarSolicitudComponent } from './expansion-panel-detallar-solicitud.component';

describe('ExpansionPanelDetallarSolicitudComponent', () => {
  let component: ExpansionPanelDetallarSolicitudComponent;
  let fixture: ComponentFixture<ExpansionPanelDetallarSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpansionPanelDetallarSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpansionPanelDetallarSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
