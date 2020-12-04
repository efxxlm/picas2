import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramacionObraVerificarRequisitosComponent } from './programacion-obra-verificar-requisitos.component';

describe('ProgramacionObraVerificarRequisitosComponent', () => {
  let component: ProgramacionObraVerificarRequisitosComponent;
  let fixture: ComponentFixture<ProgramacionObraVerificarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramacionObraVerificarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramacionObraVerificarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
