import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiagnosticoVerificarRequisitosComponent } from './diagnostico-verificar-requisitos.component';

describe('DiagnosticoVerificarRequisitosComponent', () => {
  let component: DiagnosticoVerificarRequisitosComponent;
  let fixture: ComponentFixture<DiagnosticoVerificarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiagnosticoVerificarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiagnosticoVerificarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
