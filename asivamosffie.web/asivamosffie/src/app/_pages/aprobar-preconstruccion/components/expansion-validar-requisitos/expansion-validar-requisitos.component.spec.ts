import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpansionValidarRequisitosComponent } from './expansion-validar-requisitos.component';

describe('ExpansionValidarRequisitosComponent', () => {
  let component: ExpansionValidarRequisitosComponent;
  let fixture: ComponentFixture<ExpansionValidarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpansionValidarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpansionValidarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
