import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistroNuevoProcesoJudicialComponent } from './registro-nuevo-proceso-judicial.component';

describe('RegistroNuevoProcesoJudicialComponent', () => {
  let component: RegistroNuevoProcesoJudicialComponent;
  let fixture: ComponentFixture<RegistroNuevoProcesoJudicialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistroNuevoProcesoJudicialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistroNuevoProcesoJudicialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
