import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesSupervisorComponent } from './observaciones-supervisorcomponent';

describe('ObservacionesSupervisorComponent', () => {
  let component: ObservacionesSupervisorComponent;
  let fixture: ComponentFixture<ObservacionesSupervisorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesSupervisorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesSupervisorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
