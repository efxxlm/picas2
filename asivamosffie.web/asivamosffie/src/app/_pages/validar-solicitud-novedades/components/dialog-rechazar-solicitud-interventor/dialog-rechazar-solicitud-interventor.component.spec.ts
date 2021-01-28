import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogRechazarSolicitudInterventorComponent } from './dialog-rechazar-solicitud-interventor.component';

describe('DialogRechazarSolicitudInterventorComponent', () => {
  let component: DialogRechazarSolicitudInterventorComponent;
  let fixture: ComponentFixture<DialogRechazarSolicitudInterventorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogRechazarSolicitudInterventorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogRechazarSolicitudInterventorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
