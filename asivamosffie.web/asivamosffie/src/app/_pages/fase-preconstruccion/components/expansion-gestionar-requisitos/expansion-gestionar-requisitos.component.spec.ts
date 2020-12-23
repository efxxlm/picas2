import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpansionGestionarRequisitosComponent } from './expansion-gestionar-requisitos.component';

describe('ExpansionGestionarRequisitosComponent', () => {
  let component: ExpansionGestionarRequisitosComponent;
  let fixture: ComponentFixture<ExpansionGestionarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpansionGestionarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpansionGestionarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
