import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FichaContratoComponent } from './ficha-contrato.component';

describe('FichaContratoComponent', () => {
  let component: FichaContratoComponent;
  let fixture: ComponentFixture<FichaContratoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FichaContratoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FichaContratoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
