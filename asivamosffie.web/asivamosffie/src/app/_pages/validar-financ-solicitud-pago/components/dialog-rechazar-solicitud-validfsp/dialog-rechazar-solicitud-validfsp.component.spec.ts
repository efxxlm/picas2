import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogRechazarSolicitudValidfspComponent } from './dialog-rechazar-solicitud-validfsp.component';

describe('DialogRechazarSolicitudValidfspComponent', () => {
  let component: DialogRechazarSolicitudValidfspComponent;
  let fixture: ComponentFixture<DialogRechazarSolicitudValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogRechazarSolicitudValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogRechazarSolicitudValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
