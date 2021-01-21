import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarMesaDeTrabajoActComponent } from './form-registrar-mesa-de-trabajo-act.component';

describe('FormRegistrarMesaDeTrabajoActComponent', () => {
  let component: FormRegistrarMesaDeTrabajoActComponent;
  let fixture: ComponentFixture<FormRegistrarMesaDeTrabajoActComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarMesaDeTrabajoActComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarMesaDeTrabajoActComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
