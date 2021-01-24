import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarAvanceSemanalComponent } from './validar-avance-semanal.component';

describe('ValidarAvanceSemanalComponent', () => {
  let component: ValidarAvanceSemanalComponent;
  let fixture: ComponentFixture<ValidarAvanceSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarAvanceSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarAvanceSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
