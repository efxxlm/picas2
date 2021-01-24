import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogDevolverSolicitudComponent } from './dialog-devolver-solicitud.component';

describe('DialogDevolverSolicitudComponent', () => {
  let component: DialogDevolverSolicitudComponent;
  let fixture: ComponentFixture<DialogDevolverSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogDevolverSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogDevolverSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
