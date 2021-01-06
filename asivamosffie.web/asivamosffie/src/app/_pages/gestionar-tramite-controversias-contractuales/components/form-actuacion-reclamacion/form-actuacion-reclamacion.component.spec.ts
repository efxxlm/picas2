import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormActuacionReclamacionComponent } from './form-actuacion-reclamacion.component';

describe('FormActuacionReclamacionComponent', () => {
  let component: FormActuacionReclamacionComponent;
  let fixture: ComponentFixture<FormActuacionReclamacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormActuacionReclamacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormActuacionReclamacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
