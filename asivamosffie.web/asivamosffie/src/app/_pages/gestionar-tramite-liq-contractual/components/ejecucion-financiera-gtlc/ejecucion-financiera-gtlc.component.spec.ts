import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EjecucionFinancieraGtlcComponent } from './ejecucion-financiera-gtlc.component';

describe('EjecucionFinancieraGtlcComponent', () => {
  let component: EjecucionFinancieraGtlcComponent;
  let fixture: ComponentFixture<EjecucionFinancieraGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EjecucionFinancieraGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EjecucionFinancieraGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
