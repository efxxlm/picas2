import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarListaChequeoComponent } from './gestionar-lista-chequeo.component';

describe('GestionarListaChequeoComponent', () => {
  let component: GestionarListaChequeoComponent;
  let fixture: ComponentFixture<GestionarListaChequeoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarListaChequeoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarListaChequeoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
