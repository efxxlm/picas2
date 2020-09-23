import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistroModificacionContractualComponent } from './form-registro-modificacion-contractual.component';

describe('FormRegistroModificacionContractualComponent', () => {
  let component: FormRegistroModificacionContractualComponent;
  let fixture: ComponentFixture<FormRegistroModificacionContractualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistroModificacionContractualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistroModificacionContractualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
