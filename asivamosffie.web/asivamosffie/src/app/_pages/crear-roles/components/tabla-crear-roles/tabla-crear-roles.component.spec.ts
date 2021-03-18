import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaCrearRolesComponent } from './tabla-crear-roles.component';

describe('TablaCrearRolesComponent', () => {
  let component: TablaCrearRolesComponent;
  let fixture: ComponentFixture<TablaCrearRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaCrearRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaCrearRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
