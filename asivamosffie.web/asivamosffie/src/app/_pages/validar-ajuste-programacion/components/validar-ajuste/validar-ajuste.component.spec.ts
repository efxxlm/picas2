import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarAjusteComponent } from './validar-ajuste.component';

describe('ValidarAjusteComponent', () => {
  let component: ValidarAjusteComponent;
  let fixture: ComponentFixture<ValidarAjusteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarAjusteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarAjusteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
