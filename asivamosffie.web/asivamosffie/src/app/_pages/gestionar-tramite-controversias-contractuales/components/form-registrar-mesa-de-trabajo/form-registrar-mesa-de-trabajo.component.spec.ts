import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarMesaDeTrabajoComponent } from './form-registrar-mesa-de-trabajo.component';

describe('FormRegistrarMesaDeTrabajoComponent', () => {
  let component: FormRegistrarMesaDeTrabajoComponent;
  let fixture: ComponentFixture<FormRegistrarMesaDeTrabajoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarMesaDeTrabajoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarMesaDeTrabajoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
