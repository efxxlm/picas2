import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrearRolesComponent } from './crear-roles.component';

describe('CrearRolesComponent', () => {
  let component: CrearRolesComponent;
  let fixture: ComponentFixture<CrearRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrearRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrearRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
