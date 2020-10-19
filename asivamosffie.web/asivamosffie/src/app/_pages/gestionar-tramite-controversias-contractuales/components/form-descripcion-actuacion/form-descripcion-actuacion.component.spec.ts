import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDescripcionActuacionComponent } from './form-descripcion-actuacion.component';

describe('FormDescripcionActuacionComponent', () => {
  let component: FormDescripcionActuacionComponent;
  let fixture: ComponentFixture<FormDescripcionActuacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDescripcionActuacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDescripcionActuacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
