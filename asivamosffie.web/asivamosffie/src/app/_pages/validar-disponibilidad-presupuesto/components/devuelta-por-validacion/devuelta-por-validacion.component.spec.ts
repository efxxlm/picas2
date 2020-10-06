import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DevueltaPorValidacionComponent } from './devuelta-por-validacion.component';

describe('DevueltaPorValidacionComponent', () => {
  let component: DevueltaPorValidacionComponent;
  let fixture: ComponentFixture<DevueltaPorValidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DevueltaPorValidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DevueltaPorValidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
