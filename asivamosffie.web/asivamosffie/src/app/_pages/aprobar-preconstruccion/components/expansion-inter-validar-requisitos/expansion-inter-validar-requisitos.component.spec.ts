import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpansionInterValidarRequisitosComponent } from './expansion-inter-validar-requisitos.component';

describe('ExpansionInterValidarRequisitosComponent', () => {
  let component: ExpansionInterValidarRequisitosComponent;
  let fixture: ComponentFixture<ExpansionInterValidarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpansionInterValidarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpansionInterValidarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
