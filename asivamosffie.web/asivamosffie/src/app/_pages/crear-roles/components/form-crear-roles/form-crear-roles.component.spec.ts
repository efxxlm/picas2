import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCrearRolesComponent } from './form-crear-roles.component';

describe('FormCrearRolesComponent', () => {
  let component: FormCrearRolesComponent;
  let fixture: ComponentFixture<FormCrearRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCrearRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCrearRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
