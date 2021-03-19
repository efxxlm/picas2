import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EjecucionFinancieraRlcComponent } from './ejecucion-financiera-rlc.component';

describe('EjecucionFinancieraRlcComponent', () => {
  let component: EjecucionFinancieraRlcComponent;
  let fixture: ComponentFixture<EjecucionFinancieraRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EjecucionFinancieraRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EjecucionFinancieraRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
