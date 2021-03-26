import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcesosDefensaJudicialComponent } from './procesos-defensa-judicial.component';

describe('ProcesosDefensaJudicialComponent', () => {
  let component: ProcesosDefensaJudicialComponent;
  let fixture: ComponentFixture<ProcesosDefensaJudicialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcesosDefensaJudicialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcesosDefensaJudicialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
