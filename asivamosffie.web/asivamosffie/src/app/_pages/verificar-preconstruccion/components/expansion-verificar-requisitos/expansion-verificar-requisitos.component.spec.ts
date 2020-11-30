import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpansionVerificarRequisitosComponent } from './expansion-verificar-requisitos.component';

describe('ExpansionVerificarRequisitosComponent', () => {
  let component: ExpansionVerificarRequisitosComponent;
  let fixture: ComponentFixture<ExpansionVerificarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpansionVerificarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpansionVerificarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
