import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanesProgramasComponent } from './planes-programas.component';

describe('PlanesProgramasComponent', () => {
  let component: PlanesProgramasComponent;
  let fixture: ComponentFixture<PlanesProgramasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanesProgramasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanesProgramasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
