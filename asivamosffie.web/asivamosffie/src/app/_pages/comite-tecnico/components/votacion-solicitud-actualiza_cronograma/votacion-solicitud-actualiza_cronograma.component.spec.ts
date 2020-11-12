import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VotacionSolicitudActualizaCronogramaComponent } from './votacion-solicitud-actualiza_cronograma.component';

describe('VotacionSolicitudActualizaCronogramaComponent', () => {
  let component: VotacionSolicitudActualizaCronogramaComponent;
  let fixture: ComponentFixture<VotacionSolicitudActualizaCronogramaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VotacionSolicitudActualizaCronogramaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VotacionSolicitudActualizaCronogramaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
