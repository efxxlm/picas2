import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarSeguimientoDiarioComponent } from './validar-seguimiento-diario.component';

describe('ValidarSeguimientoDiarioComponent', () => {
  let component: ValidarSeguimientoDiarioComponent;
  let fixture: ComponentFixture<ValidarSeguimientoDiarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarSeguimientoDiarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarSeguimientoDiarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
