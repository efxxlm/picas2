import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarProcesosDefensaJudicialComponent } from './gestionar-procesos-defensa-judicial.component';

describe('GestionarProcesosDefensaJudicialComponent', () => {
  let component: GestionarProcesosDefensaJudicialComponent;
  let fixture: ComponentFixture<GestionarProcesosDefensaJudicialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarProcesosDefensaJudicialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarProcesosDefensaJudicialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
