import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarListaChequeoComponent } from './validar-lista-chequeo.component';

describe('ValidarListaChequeoComponent', () => {
  let component: ValidarListaChequeoComponent;
  let fixture: ComponentFixture<ValidarListaChequeoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarListaChequeoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarListaChequeoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
