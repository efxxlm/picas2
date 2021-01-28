import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogRechazarSolicitudVfspComponent } from './dialog-rechazar-solicitud-vfsp.component';

describe('DialogRechazarSolicitudVfspComponent', () => {
  let component: DialogRechazarSolicitudVfspComponent;
  let fixture: ComponentFixture<DialogRechazarSolicitudVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogRechazarSolicitudVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogRechazarSolicitudVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
