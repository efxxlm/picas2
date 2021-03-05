import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CriteriosPagoComponent } from './criterios-pago.component';

describe('CriteriosPagoComponent', () => {
  let component: CriteriosPagoComponent;
  let fixture: ComponentFixture<CriteriosPagoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CriteriosPagoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CriteriosPagoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
