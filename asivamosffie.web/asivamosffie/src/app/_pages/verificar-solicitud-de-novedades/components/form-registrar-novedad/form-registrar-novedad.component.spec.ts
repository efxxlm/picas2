import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarNovedadComponent } from './form-registrar-novedad.component';

describe('FormRegistrarNovedadComponent', () => {
  let component: FormRegistrarNovedadComponent;
  let fixture: ComponentFixture<FormRegistrarNovedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarNovedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarNovedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
