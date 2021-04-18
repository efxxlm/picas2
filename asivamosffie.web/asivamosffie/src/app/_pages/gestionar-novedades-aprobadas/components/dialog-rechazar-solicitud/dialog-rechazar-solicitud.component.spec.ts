import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogRechazarSolicitudComponent } from './dialog-rechazar-solicitud.component';

describe('DialogRechazarSolicitudComponent', () => {
  let component: DialogRechazarSolicitudComponent;
  let fixture: ComponentFixture<DialogRechazarSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogRechazarSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogRechazarSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
