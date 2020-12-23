import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidacionRequisitosInterventoriaArtcComponent } from './form-validacion-requisitos-interventoria-artc.component';

describe('FormValidacionRequisitosInterventoriaArtcComponent', () => {
  let component: FormValidacionRequisitosInterventoriaArtcComponent;
  let fixture: ComponentFixture<FormValidacionRequisitosInterventoriaArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormValidacionRequisitosInterventoriaArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormValidacionRequisitosInterventoriaArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
