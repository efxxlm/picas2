import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogDevolverSolicitudInterventorComponent } from './dialog-devolver-solicitud-interventor.component';

describe('DialogDevolverSolicitudInterventorComponent', () => {
  let component: DialogDevolverSolicitudInterventorComponent;
  let fixture: ComponentFixture<DialogDevolverSolicitudInterventorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogDevolverSolicitudInterventorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogDevolverSolicitudInterventorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
