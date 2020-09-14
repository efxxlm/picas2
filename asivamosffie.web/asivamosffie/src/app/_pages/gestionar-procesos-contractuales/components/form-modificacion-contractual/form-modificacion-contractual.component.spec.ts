import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormModificacionContractualComponent } from './form-modificacion-contractual.component';

describe('FormModificacionContractualComponent', () => {
  let component: FormModificacionContractualComponent;
  let fixture: ComponentFixture<FormModificacionContractualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormModificacionContractualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormModificacionContractualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
