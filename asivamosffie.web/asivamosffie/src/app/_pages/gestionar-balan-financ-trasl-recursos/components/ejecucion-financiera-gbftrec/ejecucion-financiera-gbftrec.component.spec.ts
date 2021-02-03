import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EjecucionFinancieraGbftrecComponent } from './ejecucion-financiera-gbftrec.component';

describe('EjecucionFinancieraGbftrecComponent', () => {
  let component: EjecucionFinancieraGbftrecComponent;
  let fixture: ComponentFixture<EjecucionFinancieraGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EjecucionFinancieraGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EjecucionFinancieraGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
