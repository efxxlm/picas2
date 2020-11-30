import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DevueltaPorCoordinacionComponent } from './devuelta-por-coordinacion.component';

describe('DevueltaPorCoordinacionComponent', () => {
  let component: DevueltaPorCoordinacionComponent;
  let fixture: ComponentFixture<DevueltaPorCoordinacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DevueltaPorCoordinacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DevueltaPorCoordinacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
