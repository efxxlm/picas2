import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetallePlanesProgramasComponent } from './detalle-planes-programas.component';

describe('DetallePlanesProgramasComponent', () => {
  let component: DetallePlanesProgramasComponent;
  let fixture: ComponentFixture<DetallePlanesProgramasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetallePlanesProgramasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetallePlanesProgramasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
