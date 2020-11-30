import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VotacionSolicitudMultipleComponent } from './votacion-solicitud-multiple.component';

describe('VotacionSolicitudMultipleComponent', () => {
  let component: VotacionSolicitudMultipleComponent;
  let fixture: ComponentFixture<VotacionSolicitudMultipleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VotacionSolicitudMultipleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VotacionSolicitudMultipleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
