import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarRequisitosGtlcComponent } from './verificar-requisitos-gtlc.component';

describe('VerificarRequisitosGtlcComponent', () => {
  let component: VerificarRequisitosGtlcComponent;
  let fixture: ComponentFixture<VerificarRequisitosGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarRequisitosGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarRequisitosGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
