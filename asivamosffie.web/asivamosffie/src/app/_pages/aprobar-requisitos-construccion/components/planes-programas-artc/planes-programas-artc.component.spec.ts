import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanesProgramasArtcComponent } from './planes-programas-artc.component';

describe('PlanesProgramasArtcComponent', () => {
  let component: PlanesProgramasArtcComponent;
  let fixture: ComponentFixture<PlanesProgramasArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanesProgramasArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanesProgramasArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
