import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DevueltaPorCoordinacionFinancieraComponent } from './devuelta-por-coordinacion-financiera.component';

describe('DevueltaPorCoordinacionFinancieraComponent', () => {
  let component: DevueltaPorCoordinacionFinancieraComponent;
  let fixture: ComponentFixture<DevueltaPorCoordinacionFinancieraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DevueltaPorCoordinacionFinancieraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DevueltaPorCoordinacionFinancieraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
