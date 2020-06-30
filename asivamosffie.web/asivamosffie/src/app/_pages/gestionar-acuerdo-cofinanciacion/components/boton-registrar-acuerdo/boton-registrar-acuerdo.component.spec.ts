import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BotonRegistrarAcuerdoComponent } from './boton-registrar-acuerdo.component';

describe('BotonRegistrarAcuerdoComponent', () => {
  let component: BotonRegistrarAcuerdoComponent;
  let fixture: ComponentFixture<BotonRegistrarAcuerdoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BotonRegistrarAcuerdoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BotonRegistrarAcuerdoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
