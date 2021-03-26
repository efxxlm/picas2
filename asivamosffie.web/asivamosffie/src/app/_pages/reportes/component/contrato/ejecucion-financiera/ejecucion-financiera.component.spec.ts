import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EjecucionFinancieraComponent } from './ejecucion-financiera.component';

describe('EjecucionFinancieraComponent', () => {
  let component: EjecucionFinancieraComponent;
  let fixture: ComponentFixture<EjecucionFinancieraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EjecucionFinancieraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EjecucionFinancieraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
