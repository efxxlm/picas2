import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidacionRequisitosObraArtcComponent } from './form-validacion-requisitos-obra-artc.component';

describe('FormValidacionRequisitosObraArtcComponent', () => {
  let component: FormValidacionRequisitosObraArtcComponent;
  let fixture: ComponentFixture<FormValidacionRequisitosObraArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormValidacionRequisitosObraArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormValidacionRequisitosObraArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
